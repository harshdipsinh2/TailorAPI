using Microsoft.EntityFrameworkCore;

public class OrderRepository
{
    private readonly TailorDbContext _context;
    public OrderRepository(TailorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrders() => await _context.Orders.ToListAsync();
    public async Task<Order> GetOrderById(int id) => await _context.Orders.FindAsync(id);
    public async Task AddOrder(Order order) { _context.Orders.Add(order); await _context.SaveChangesAsync(); }
    public async Task UpdateOrder(Order order) { _context.Orders.Update(order); await _context.SaveChangesAsync(); }
    public async Task DeleteOrder(Order order) { _context.Orders.Remove(order); await _context.SaveChangesAsync(); }
}