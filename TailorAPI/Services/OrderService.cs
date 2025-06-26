using Microsoft.EntityFrameworkCore;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Models;
using TailorAPI.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Stripe;
using Stripe.Checkout;
using TailorAPI.DTO.Request;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services;


namespace TailorAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly TailorDbContext _context;
        private readonly TwilioService _twilioService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(OrderRepository orderRepository, TailorDbContext context, TwilioService twilioService,IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _context = context;
            _twilioService = twilioService;
            _httpContextAccessor = httpContextAccessor;
        }



        // ✅ Stripe Payment Session Creation
        public async Task<string> CreateStripePaymentSessionAsync(decimal totalPrice)
        {


            Stripe.StripeConfiguration.ApiKey = "sk_test_your_secret_key"; // Replace with your actual Stripe Secret Key

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
        {
            "card",
        },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
        {
            new Stripe.Checkout.SessionLineItemOptions
            {
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    Currency = "usd", // or change it to your currency like "inr" if needed
                    UnitAmount = (long)(totalPrice * 100),
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Order Payment",
                    },
                },
                Quantity = 1,
            },
        },
                Mode = "payment",
                SuccessUrl = "https://yourdomain.com/success", // ✅ After payment success, redirect here
                CancelUrl = "https://yourdomain.com/cancel",   // ✅ If payment canceled, redirect here
            };

            var service = new Stripe.Checkout.SessionService();
            var session = await service.CreateAsync(options);

            return session.Url; // ✅ Return Stripe Payment URL to frontend
        }


        // ✅ Create Order with Calculation Logic
        public async Task<OrderResponseDto> CreateOrderAsync(int customerId, int productId, int fabricTypeId, int assignedTo, OrderRequestDto requestDto)
        {
            var shopId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("shopId")?.Value ?? "0");
            var branchId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("branchId")?.Value ?? "0");

            var shop = await _context.Shops
          .Include(s => s.Plan)
          .Include(s => s.Orders)
          .FirstOrDefaultAsync(s => s.ShopId == shopId);

            if (shop == null) throw new Exception("Shop not found");
            if (shop.Plan == null) throw new Exception("No active plan found for this shop");

            int currentOrderCount = shop.Orders
                .Count(o => o.CreatedDate >= shop.PlanStartDate && o.CreatedDate <= shop.PlanEndDate && !o.IsDeleted);

            if (currentOrderCount >= shop.Plan.MaxOrders)
                throw new Exception("Order limit reached for your current plan.");



            var product = await _context.Products.FindAsync(productId);
            var fabricType = await _context.FabricTypes.FindAsync(fabricTypeId);

            if (product == null || fabricType == null)
                throw new Exception("Invalid Product or Fabric Type");

            // 🚨 Fetch customer measurement
            var measurement = await _context.Measurements.FirstOrDefaultAsync(m => m.CustomerId == customerId);
            if (measurement == null)
                throw new Exception("Customer does not have measurements. Please add measurements first.");

            // 🚨 Auto-set FabricLength based on product type
            if (string.Equals(product.ProductType.ToString(), "upper", StringComparison.OrdinalIgnoreCase))
            {
                requestDto.FabricLength = measurement.UpperBodyMeasurement;
            }
            else if (string.Equals(product.ProductType.ToString(), "lower", StringComparison.OrdinalIgnoreCase))
            {
                requestDto.FabricLength = measurement.LowerBodyMeasurement;
            }
            else
            {
                throw new Exception("Unknown product type. ProductType must be either 'upper' or 'lower'.");
            }
            // 🚨 Calculate AvailableStock using FabricStock entries
            var totalStockIn = await _context.FabricStocks
                .Where(fs => fs.FabricTypeID == fabricTypeId)
                .SumAsync(fs => fs.StockIn);

            var totalStockUsed = await _context.FabricStocks
                .Where(fs => fs.FabricTypeID == fabricTypeId)
                .SumAsync(fs => fs.StockUse);

            var availableStock = totalStockIn - totalStockUsed;

            //if (availableStock < (decimal)requestDto.FabricLength * requestDto.Quantity)
            //    throw new Exception("Insufficient fabric stock.");

            // 🚨 Create new fabric stock entry



            //Add FabricStock Entry for Usage
            var newFabricStockEntry = new FabricStock
            {
                FabricTypeID = fabricTypeId,
                ShopId = shopId,
                BranchId = branchId,
                StockIn = 0,
                StockUse = (decimal)requestDto.FabricLength * requestDto.Quantity,
                StockAddDate = DateTime.Now
            };

            _context.FabricStocks.Add(newFabricStockEntry);



            //Update FabricType Stock
            fabricType.AvailableStock -= ((decimal)requestDto.FabricLength * requestDto.Quantity);
            _context.FabricTypes.Update(fabricType);

            // 🚨 Order Calculation Logic
            var totalPrice = ((decimal)requestDto.FabricLength * fabricType.PricePerMeter + product.MakingPrice)
                           * requestDto.Quantity;

            var order = new Order
            {
                CustomerId = customerId,
                ProductID = productId,
                ShopId = shopId,
                BranchId = branchId,
                Quantity = requestDto.Quantity,
                FabricLength = (decimal)requestDto.FabricLength,
                TotalPrice = totalPrice,
                CompletionDate = requestDto.CompletionDate,
                FabricTypeID = fabricTypeId,
                AssignedTo = assignedTo,
                AssignedAt = DateTime.UtcNow, // 🚨 Set assigned time
                OrderStatus = Enum.Parse<OrderStatus>("Pending"),
                PaymentStatus = Enum.Parse<PaymentStatus>("Pending"),
                ApprovalStatus = Enum.Parse<OrderApprovalStatus>("Pending"),
                PlanId = shop.PlanId
            };


            _context.Orders.Add(order);

            // 🚨 Assign user and update status
            var assignedUser = await _context.Users.FindAsync(assignedTo);
            if (assignedUser == null || !assignedUser.IsVerified)
            {
                throw new Exception("Assigned user must be a verified tailor or manager.");
            }

            assignedUser.UserStatus = UserStatus.Busy;
            _context.Users.Update(assignedUser);

            await _context.SaveChangesAsync();

            return new OrderResponseDto
            {
                AssignedTo = order.AssignedTo,
                OrderStatus = order.OrderStatus,
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd")
            };
        }

        public async Task<bool> UpdateOrderApprovalAsync(int orderId, int userId, OrderApprovalUpdateDTO requestDto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            //if (order.AssignedTo != userId)
            //{
            //    throw new UnauthorizedAccessException("You can only approve/reject orders assigned to you");
            //}

            order.ApprovalStatus = requestDto.ApprovalStatus;
            order.RejectionReason = requestDto.ApprovalStatus == OrderApprovalStatus.Rejected ?
                requestDto.RejectionReason : null;

            if (requestDto.ApprovalStatus == OrderApprovalStatus.Approved)
            {
                order.OrderStatus = OrderStatus.Pending;
            }

            await _orderRepository.UpdateOrderAsync(order);
            await _context.SaveChangesAsync();

            return true;
        }



        // ✅ Updated Order Logic with AssignedTo and CompletionDate Fix
        public async Task<bool> UpdateOrderAsync(int id, int productId, int fabricTypeId, int assignedTo, OrderRequestDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return false;

            var product = await _context.Products.FindAsync(productId);
            var fabricType = await _context.FabricTypes.FindAsync(fabricTypeId);

            if (product == null || fabricType == null)
                throw new Exception("Invalid Product or Fabric Type");

            // ✅ Fabric Stock Management
            var totalStockIn = await _context.FabricStocks
                .Where(fs => fs.FabricTypeID == fabricTypeId)
                .SumAsync(fs => fs.StockIn);

            var totalStockUsed = await _context.FabricStocks
                .Where(fs => fs.FabricTypeID == fabricTypeId)
                .SumAsync(fs => fs.StockUse);

            var availableStock = totalStockIn - totalStockUsed;

            //if (availableStock < (decimal)request.FabricLength * request.Quantity)
            //    throw new Exception("Insufficient fabric stock.");

            // ✅ Add new fabric usage entry
            var newFabricStockEntry = new FabricStock
            {
                FabricTypeID = fabricTypeId,
                StockIn = 0,
                StockUse = (decimal)request.FabricLength * request.Quantity,
                StockAddDate = DateTime.Now
            };

            _context.FabricStocks.Add(newFabricStockEntry);
            fabricType.AvailableStock -= ((decimal)request.FabricLength * request.Quantity);
            _context.FabricTypes.Update(fabricType);

            // ✅ Update order details
            order.ProductID = productId;
            order.FabricTypeID = fabricTypeId;
            order.FabricLength = (decimal)request.FabricLength;
            order.Quantity = request.Quantity;
            order.TotalPrice = ((decimal)request.FabricLength * fabricType.PricePerMeter + product.MakingPrice) * request.Quantity;
            order.OrderStatus = request.OrderStatus;
            order.PaymentStatus = request.paymentStatus;

            // ✅ Reassignment logic
            if (order.AssignedTo != assignedTo)
            {
                // Set previous user to available
                var previousUser = await _context.Users.FindAsync(order.AssignedTo);
                if (previousUser != null)
                {
                    previousUser.UserStatus = UserStatus.Available;
                    _context.Users.Update(previousUser);
                }

                // Assign new user
                var newAssignedUser = await _context.Users.FindAsync(assignedTo);
                if (newAssignedUser == null || !newAssignedUser.IsVerified)
                    throw new Exception("Assigned user must be a verified tailor or manager.");

                newAssignedUser.UserStatus = UserStatus.Busy;
                _context.Users.Update(newAssignedUser);

                order.AssignedTo = assignedTo;
                order.AssignedAt = DateTime.UtcNow; // ✅ Update assigned time to now

                // ✅ Reset approval status if reassigned
                order.ApprovalStatus = OrderApprovalStatus.Pending;
                order.RejectionReason = null;
            }

            await _orderRepository.UpdateOrderAsync(order);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            // 🟡 Check if completion date is changing
            var oldCompletionDate = order.CompletionDate;
            var newCompletionDate = request.CompletionDate;

            // ✅ Update status fields
            order.OrderStatus = request.OrderStatus;
            order.PaymentStatus = request.PaymentStatus;

            // ✅ Update completion date if provided
            if (newCompletionDate.HasValue)
            {
                order.CompletionDate = newCompletionDate.Value;
            }

            // ✅ Handle Assigned User Status if order completed
            if (order.OrderStatus == OrderStatus.Completed && order.PaymentStatus == PaymentStatus.Completed)
            {
                // Check if the assigned user has any other active (not completed) orders
                var hasOtherActiveOrders = await _context.Orders.AnyAsync(o =>
                    o.AssignedTo == order.AssignedTo &&
                    o.OrderStatus != OrderStatus.Completed &&
                    !o.IsDeleted &&
                    o.OrderID != order.OrderID);

                if (!hasOtherActiveOrders)
                {
                    var assignedUser = await _context.Users.FindAsync(order.AssignedTo);
                    if (assignedUser != null)
                    {
                        assignedUser.UserStatus = UserStatus.Available;
                        _context.Users.Update(assignedUser);
                    }
                }
            }

            await _orderRepository.UpdateOrderAsync(order);
            await _context.SaveChangesAsync();

            // ✅ After saving, decide and send SMS + WhatsApp
            if (newCompletionDate.HasValue)
            {
                SmsType smsType;

                if (oldCompletionDate == null)
                {
                    smsType = SmsType.Completion;
                }
                else if (newCompletionDate > oldCompletionDate)
                {
                    smsType = SmsType.Delayed;
                }
                else if (newCompletionDate < oldCompletionDate)
                {
                    smsType = SmsType.PreCompletion;
                }
                else
                {
                    // If date is the same → schedule 1-day-before notification
                    var delay = newCompletionDate.Value.AddDays(-1) - DateTime.Now;
                    if (delay.TotalMilliseconds > 0)
                    {
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(delay);
                            await _twilioService.SendWhatsappTemplateMessage(order.Customer.PhoneNumber, SmsType.Completion, order.OrderID);
                            await _twilioService.SendSmsAsync(order.Customer.PhoneNumber, $"Your order #{order.OrderID} will be ready tomorrow!");
                        });
                    }

                    return true;
                }

                // Get customer phone
                var customer = await _context.Customers.FindAsync(order.CustomerId);
                if (customer != null && !string.IsNullOrEmpty(customer.PhoneNumber))
                {
                    await _twilioService.SendWhatsappTemplateMessage(customer.PhoneNumber, smsType, order.OrderID);

                    // Simple SMS content (or make dynamic based on type)
                    string smsMessage = smsType switch
                    {
                        SmsType.Delayed => $"Dear {customer.FullName}, We're experiencing slight delays with your order. New delivery date: {newCompletionDate:yyyy-MM-dd}.",
                        SmsType.PreCompletion => $"Good news, {customer.FullName}!  Your order is  ready before the completion date , please visit us or contact for delivery arrangements . New EarlierDate: {newCompletionDate:dd-MM-yyyy}",
                        SmsType.Completion => $"Reminder: Dear {customer.FullName}, your order will be ready tomorrow. Please visit us or contact for delivery arrangements.",
                        _ => "Order update."
                    };

                    await _twilioService.SendSmsAsync(customer.PhoneNumber, smsMessage);
                }
            }

            return true;
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _orderRepository.GetTotalRevenueAsync();
        }



        public async Task<bool> SoftDeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return false;

            order.IsDeleted = true;


            // 🚨 Update UserStatus to "Available" when order is deleted
            var assignedUser = await _context.Users.FindAsync(order.AssignedTo);
            if (assignedUser != null)
            {
                assignedUser.UserStatus = UserStatus.Available;
                _context.Users.Update(assignedUser);
            }

            await _orderRepository.UpdateOrderAsync(order);
            await _context.SaveChangesAsync();

            return true;
        }

        // ✅ Corrected Response to Display Non-null Values
        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .Include(o => o.Customer)
                .Include(o => o.Assigned) // Include the Assigned User
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return null;

            return new OrderResponseDto
            {
                CustomerID = order.CustomerId,   // ✅ Added ID reference
                ProductID = order.ProductID,     // ✅ Added ID reference
                FabricTypeID = order.FabricTypeID,       // ✅ Added ID reference

                CustomerName = order.Customer?.FullName,
                ProductName = order.Product?.ProductName,
                FabricName = order.fabricType?.FabricName ?? "N/A", // ✅ Display "N/A" if Fabric is missing

                FabricLength = order.FabricLength,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                AssignedTo = order.AssignedTo,
                AssignedToName = order.Assigned?.Name, // Map the Assigned User's Name
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus
            };
        }


        // ✅ Corrected for Non-null Values in List
        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync(int userId, string role)
        {
            var query = _context.Orders
                .IgnoreQueryFilters()
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .Include(o => o.Customer)
                .Include(o => o.Assigned)
                .Include(o => o.Branch)
                .Include(o => o.Shop)
                .AsQueryable();

            if (role == "Tailor")
            {
                var user = _httpContextAccessor.HttpContext.User;
                var shopId = int.Parse(user.FindFirst("shopId")?.Value ?? "0");
                var branchId = int.Parse(user.FindFirst("branchId")?.Value ?? "0");

                query = query.Where(o => o.AssignedTo == userId);
            }

            var orders = await query.ToListAsync();

            return orders.Select(order => new OrderResponseDto
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerId,
                ProductID = order.ProductID,
                FabricTypeID = order.FabricTypeID,
                CustomerName = order.Customer?.FullName,
                ProductName = order.Product?.ProductName,
                FabricName = order.fabricType?.FabricName ?? "N/A",
                FabricLength = order.FabricLength,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd"),

                BranchId = order.BranchId,
                BranchName = order.Branch?.BranchName,   // ✅ Get Branch Name
                ShopId = order.ShopId,
                ShopName = order.Shop?.ShopName,

                AssignedTo = order.AssignedTo,
                AssignedAt = order.AssignedAt,
                AssignedToName = order.Assigned?.Name,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                ApprovalStatus = order.ApprovalStatus,
                RejectionReason = order.RejectionReason

            }).ToList();
        }
    
         public async Task<IEnumerable<OrderResponseDto>> GetRejectedOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .Include(o => o.Customer)
                .Include(o => o.Assigned)
                .Where(o => o.ApprovalStatus == OrderApprovalStatus.Rejected && !o.IsDeleted)
                .ToListAsync();

            return orders.Select(order => new OrderResponseDto
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerId,
                ProductID = order.ProductID,
                FabricTypeID = order.FabricTypeID,
                CustomerName = order.Customer?.FullName,
                ProductName = order.Product?.ProductName,
                FabricName = order.fabricType?.FabricName ?? "N/A",
                FabricLength = order.FabricLength,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                AssignedTo = order.AssignedTo,
                AssignedToName = order.Assigned?.Name,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                ApprovalStatus = order.ApprovalStatus,
                RejectionReason = order.RejectionReason // ✅ So admin sees why tailor rejected it
            });
        }
        public async Task<bool> ReassignRejectedOrderAsync(int orderId, ReassignOrderDTO dto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.ApprovalStatus != OrderApprovalStatus.Rejected || order.IsDeleted)
                return false;

            var previousTailor = await _context.Users.FindAsync(order.AssignedTo);
            if (previousTailor != null)
            {
                previousTailor.UserStatus = UserStatus.Available;
                _context.Users.Update(previousTailor);
            }

            var newTailor = await _context.Users.FindAsync(dto.UserID);
            if (newTailor == null || !newTailor.IsVerified)
                throw new Exception("New tailor must be a verified user.");

            // Update order
            order.AssignedTo = dto.UserID;
            order.ApprovalStatus = OrderApprovalStatus.Pending;
            order.OrderStatus = OrderStatus.Pending;
            order.RejectionReason = null;

            newTailor.UserStatus = UserStatus.Busy;

            _context.Users.Update(newTailor);
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> RejectUnapprovedOrdersAfter24HoursAsync()
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-24);  // 1 minute ago, for quick test

            var ordersToReject = await _context.Orders
                .Where(o =>
                    o.ApprovalStatus == OrderApprovalStatus.Pending &&
                    o.AssignedAt != null &&
                    o.AssignedAt <= cutoffTime &&
                    !o.IsDeleted)   
                .ToListAsync();

            foreach (var order in ordersToReject)
            {
                order.ApprovalStatus = OrderApprovalStatus.Rejected;
                order.RejectionReason = "Automatically rejected due to no approval/rejection within 24 hours.";
                order.OrderStatus = OrderStatus.Pending; // Optional, depends on your flow

                // Free assigned user
                var assignedUser = await _context.Users.FindAsync(order.AssignedTo);
                if (assignedUser != null)
                {
                    assignedUser.UserStatus = UserStatus.Available;
                    _context.Users.Update(assignedUser);
                }
            }

            _context.Orders.UpdateRange(ordersToReject);
            await _context.SaveChangesAsync();

            return ordersToReject.Count;
        }

    }
}