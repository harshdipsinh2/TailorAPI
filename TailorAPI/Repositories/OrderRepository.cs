using TailorAPI.Models;
using Microsoft.EntityFrameworkCore;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Repositories;

namespace TailorAPI.Repositories
{
    public class OrderRepository
    {
        private readonly TailorDbContext _context;

        public OrderRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Where(o => !o.IsDeleted) // Exclude soft-deleted orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Where(o => !o.IsDeleted) // Exclude soft-deleted orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .FirstOrDefaultAsync(o => o.OrderID == id);
        }



        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}