using TailorAPI.Models;
using Microsoft.EntityFrameworkCore;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Repositories;

namespace TailorAPI.Repositories
{
    public class ManagerRepository
    {
        private readonly TailorDbContext _context;

        public ManagerRepository(TailorDbContext context)
        {
            _context = context;
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

    }
}