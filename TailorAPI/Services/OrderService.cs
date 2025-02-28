using System;
using TailorAPI.Models;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly TailorDbContext _context;

        public OrderService(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddOrder(int customerID, int productID, int employeeID, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == productID);
            if (product == null)
            {
                return "Invalid Product ID.";
            }

            string orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{_context.Orders.Count() + 1}";

            var order = new Order
            {
                OrderNumber = orderNumber,
                CustomerID = customerID,
                ProductID = productID,
                EmployeeID = employeeID,
                Quantity = quantity,
                TotalPrice = product.Price * quantity
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return $"Order placed successfully. Order Number: {orderNumber}";
        }

        public async Task<List<Order>> GetOrders()
        {
            return await Task.FromResult(_context.Orders.ToList());
        }
    }

}
