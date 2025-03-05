using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class OrderRepository
    {
        private readonly TailorDbContext _context;

        public OrderRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.Include(o => o.Customer)
                                        .Include(o => o.Product)
                                        .ToListAsync();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders.Include(o => o.Customer)
                                        .Include(o => o.Product)
                                        .FirstOrDefaultAsync(o => o.OrderID == id);
        }

        public async Task AddOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
