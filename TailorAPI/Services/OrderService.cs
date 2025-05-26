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



namespace TailorAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly TailorDbContext _context;

        public OrderService(OrderRepository orderRepository, TailorDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
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

            if (availableStock < (decimal)requestDto.FabricLength * requestDto.Quantity)
                throw new Exception("Insufficient fabric stock.");

            // 🚨 Create new fabric stock entry
            var newFabricStockEntry = new FabricStock
            {
                FabricTypeID = fabricTypeId,
                StockIn = 0,
                StockUse = (decimal)requestDto.FabricLength * requestDto.Quantity,
                StockAddDate = DateTime.Now
            };

            _context.FabricStocks.Add(newFabricStockEntry);

            fabricType.AvailableStock -= ((decimal)requestDto.FabricLength * requestDto.Quantity);
            _context.FabricTypes.Update(fabricType);

            // 🚨 Order Calculation Logic
            var totalPrice = ((decimal)requestDto.FabricLength * fabricType.PricePerMeter + product.MakingPrice)
                           * requestDto.Quantity;

            var order = new Order
            {
                CustomerId = customerId,
                ProductID = productId,
                Quantity = requestDto.Quantity,
                FabricLength = (decimal)requestDto.FabricLength,
                TotalPrice = totalPrice,
                CompletionDate = requestDto.CompletionDate,
                FabricTypeID = fabricTypeId,
                AssignedTo = assignedTo,
                OrderStatus = Enum.Parse<OrderStatus>("Pending"),
                PaymentStatus = Enum.Parse<PaymentStatus>("Pending"),
                ApprovalStatus = Enum.Parse<OrderApprovalStatus>("Pending"),
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

        // Add this new method for approval/rejection
        public async Task<bool> UpdateOrderApprovalAsync(int orderId, OrderApprovalUpdateDTO RequestDTO)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            // Only allow the assigned tailor to approve/reject
            if (order.AssignedTo != RequestDTO.UserID)
            {
                throw new UnauthorizedAccessException("You can only approve/reject orders assigned to you");
            }

            order.ApprovalStatus = RequestDTO.ApprovalStatus;
            order.RejectionReason = RequestDTO.ApprovalStatus == OrderApprovalStatus.Rejected ?
                RequestDTO.RejectionReason : null;

            if (RequestDTO.ApprovalStatus == OrderApprovalStatus.Approved)
            {
                order.OrderStatus = OrderStatus.Pending; // Ready for work
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

            if (availableStock < (decimal)request.FabricLength)
                throw new Exception("Insufficient fabric stock.");

            // ✅ Update Fabric Stock and Available Stock
            var newFabricStockEntry = new FabricStock
            {
                FabricTypeID = fabricTypeId,
                StockIn = 0,
                StockUse = (decimal)request.FabricLength * (decimal)request.Quantity,
                StockAddDate = DateTime.Now
            };

            _context.FabricStocks.Add(newFabricStockEntry);

            fabricType.AvailableStock -= ((decimal)request.FabricLength * (decimal)request.Quantity);
            _context.FabricTypes.Update(fabricType);

            // ✅ Order Details Update
            order.FabricTypeID = fabricTypeId;
            order.FabricLength = (decimal)request.FabricLength;
            order.Quantity = request.Quantity;
            order.TotalPrice = ((decimal)request.FabricLength * fabricType.PricePerMeter + (decimal)product.MakingPrice)
                           * ((decimal)request.Quantity);

            order.OrderStatus = request.OrderStatus;
            order.PaymentStatus = request.paymentStatus;

            // ✅ Assigned User Status Management
            if (order.AssignedTo != assignedTo)
            {
                var previousUser = await _context.Users.FindAsync(order.AssignedTo);
                if (previousUser != null)
                {
                    previousUser.UserStatus = UserStatus.Available;
                    _context.Users.Update(previousUser);
                }
                var newAssignedUser = await _context.Users.FindAsync(assignedTo);
                if (newAssignedUser == null || !newAssignedUser.IsVerified)
                {
                    throw new Exception("Assigned user must be a verified tailor or manager.");
                }

                newAssignedUser.UserStatus = UserStatus.Busy;
                _context.Users.Update(newAssignedUser);

                order.AssignedTo = assignedTo;
            }
            await _orderRepository.UpdateOrderAsync(order);
            await _context.SaveChangesAsync();

            return true;


        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            // ✅ Update status fields
            order.OrderStatus = request.OrderStatus;
            order.PaymentStatus = request.PaymentStatus;

            // ✅ Handle Assigned User Status if order completed
            if (order.OrderStatus == OrderStatus.Completed && order.PaymentStatus == PaymentStatus.Completed)
            {
                var assignedUser = await _context.Users.FindAsync(order.AssignedTo);
                if (assignedUser != null)
                {
                    assignedUser.UserStatus = UserStatus.Available;
                    _context.Users.Update(assignedUser);
                }
            }

            await _orderRepository.UpdateOrderAsync(order);
            await _context.SaveChangesAsync();

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
                .AsQueryable();

            if (role == "Tailor")
            {
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
                OrderDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                AssignedTo = order.AssignedTo,
                AssignedToName = order.Assigned?.Name,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus
            }).ToList();
        }
    }

    }
    