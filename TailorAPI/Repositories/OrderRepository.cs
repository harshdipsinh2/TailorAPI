using Microsoft.EntityFrameworkCore;
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

        public async Task<Order> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Where(o => !o.IsDeleted)
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .Include(o => o.Assigned)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .Include(o => o.Assigned)
                .FirstOrDefaultAsync(o => o.OrderID == id && !o.IsDeleted);
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Orders
                .Where(o => o.OrderStatus == OrderStatus.Completed && o.PaymentStatus == PaymentStatus.Completed)
                .SumAsync(o => o.TotalPrice);
        }

        public async Task<bool> SoftDeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.IsDeleted = true;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetRejectedOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.fabricType)
                .Include(o => o.Assigned)
                .Where(o => o.ApprovalStatus == OrderApprovalStatus.Rejected && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<Order?> GetRawOrderByIdAsync(int id)
        {
            // Used for cases like reassignment where minimal data is needed
            return await _context.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetOrdersToRejectAsync(DateTime cutoffTime)
        {
            return await _context.Orders
                .Where(o =>
                    o.ApprovalStatus == OrderApprovalStatus.Pending &&
                    o.AssignedAt != null &&
                    o.AssignedAt <= cutoffTime &&
                    !o.IsDeleted)
                .ToListAsync();
        }

        public async Task UpdateOrdersAsync(List<Order> orders)
        {
            _context.Orders.UpdateRange(orders);
            await _context.SaveChangesAsync();
        }

    }
}
