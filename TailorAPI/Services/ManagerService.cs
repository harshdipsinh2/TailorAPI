using Microsoft.EntityFrameworkCore;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Models;
using TailorAPI.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ManagerRepository _managerRepository;
        private readonly OrderRepository _orderRepository;
        private readonly TailorDbContext _context;

        public ManagerService(OrderRepository orderRepository, ManagerRepository managerRepository,TailorDbContext context)
        {
            _managerRepository = managerRepository;
            _orderRepository = orderRepository;
            _context = context;
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
        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .Include(o => o.Customer)
                .Include(o => o.Assigned) // Include the Assigned User
                .ToListAsync();

            return orders.Select(order => new OrderResponseDto
            {
                CustomerID = order.CustomerId,
                ProductID = order.ProductID,
                FabricTypeID = order.FabricTypeID,

                CustomerName = order.Customer?.FullName,
                ProductName = order.Product?.ProductName,
                FabricName = order.fabricType?.FabricName ?? "N/A", // ✅ Display "N/A" if Fabric is missing

                FabricLength = order.FabricLength,
                Quantity = order.Quantity,
                //TotalPrice = order.TotalPrice,
                OrderDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                CompletionDate = order.CompletionDate?.ToString("yyyy-MM-dd"),
                AssignedTo = order.AssignedTo,
                AssignedToName = order.Assigned?.Name, // Map the Assigned User's Name
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus
            }).ToList();
        }

    }
}
