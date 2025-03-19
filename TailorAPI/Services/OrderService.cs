using Microsoft.EntityFrameworkCore;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Models;
using TailorAPI.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

        // ✅ Create Order with Calculation Logic
        public async Task<OrderResponseDto> CreateOrderAsync(int customerId, int productId, int fabricId, OrderRequestDto requestDto)
        {
            var product = await _context.Products.FindAsync(productId);
            var fabric = await _context.Fabrics.FindAsync(fabricId);

            if (product == null || fabric == null)
                throw new Exception("Invalid Product or Fabric");

            var totalPrice = ((decimal)requestDto.FabricLength * fabric.PricePerMeter)
                           + ((decimal)product.MakingPrice * (decimal)requestDto.Quantity);

            var order = new Order
            {
                CustomerId = customerId,
                ProductID = productId,
                Quantity = requestDto.Quantity,
                TotalPrice = totalPrice,
                CompletionDate = requestDto.CompletionDate,
                FabricID = fabricId,
                FabricLength = (decimal)requestDto.FabricLength,
                AssignedTo = requestDto.AssignedTo,
                OrderStatus = Enum.Parse<OrderStatus>("Pending"),
                PaymentStatus = Enum.Parse<PaymentStatus>("Pending")
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 🚨 Update UserStatus to "Busy"
            var assignedUser = await _context.Users.FindAsync(requestDto.AssignedTo);
            if (assignedUser != null)
            {
                assignedUser.UserStatus = UserStatus.Busy;
                _context.Users.Update(assignedUser);
                await _context.SaveChangesAsync();
            }

            return new OrderResponseDto
            {
                AssignedTo = order.AssignedTo,
                OrderStatus = order.OrderStatus,
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd")
            };
        }

        // ✅ Updated Order Logic with AssignedTo and CompletionDate Fix
        public async Task<bool> UpdateOrderAsync(int id, int productId, int fabricId, OrderRequestDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return false;

            var product = await _context.Products.FindAsync(productId);
            var fabric = await _context.Fabrics.FindAsync(fabricId);
            if (product == null || fabric == null) throw new Exception("Invalid Product or Fabric");

            order.FabricLength = (decimal)request.FabricLength;
            order.Quantity = request.Quantity;
            order.TotalPrice = ((decimal)request.FabricLength * fabric.PricePerMeter)
                + ((decimal)product.MakingPrice * (decimal)request.Quantity);

            // ✅ Update AssignedTo and UserStatus
            if (order.AssignedTo != request.AssignedTo)
            {
                var previousUser = await _context.Users.FindAsync(order.AssignedTo);
                if (previousUser != null)
                {
                    previousUser.UserStatus = UserStatus.Available;
                    _context.Users.Update(previousUser);
                }

                var newAssignedUser = await _context.Users.FindAsync(request.AssignedTo);
                if (newAssignedUser != null)
                {
                    newAssignedUser.UserStatus = UserStatus.Busy;
                    _context.Users.Update(newAssignedUser);
                }

                order.AssignedTo = request.AssignedTo;
            }

            // ✅ Update Completion Date
            order.CompletionDate = request.CompletionDate;

            await _orderRepository.UpdateOrderAsync(order);
            await _context.SaveChangesAsync();
            return true;

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
                .Include(o => o.Fabric)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return null;

            return new OrderResponseDto
            {
                CustomerID = order.CustomerId,   // ✅ Added ID reference
                ProductID = order.ProductID,     // ✅ Added ID reference
                FabricID = order.FabricID,       // ✅ Added ID reference

                CustomerName = order.Customer?.FullName,
                ProductName = order.Product?.ProductName,
                FabricName = order.Fabric?.FabricName,

                FabricLength = order.FabricLength,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd")
            };
        }


        // ✅ Corrected for Non-null Values in List
        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Fabric)
                .Include(o => o.Customer)
                .ToListAsync();

            return orders.Select(order => new OrderResponseDto
            {
                CustomerID = order.CustomerId,
                ProductID = order.ProductID,
                FabricID = order.FabricID, 

                CustomerName = order.Customer?.FullName,
                ProductName = order.Product?.ProductName,
                FabricName = order.Fabric?.FabricName,

                FabricLength = order.FabricLength,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                OrderDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd")
            }).ToList();
        }

    }
}
    