using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;

namespace TailorAPI.Repositories // Make sure this matches your namespace
{
    public class OrderRepository
    {
        private readonly TailorDbContext _context;

        public OrderRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
