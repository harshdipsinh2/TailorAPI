using Microsoft.EntityFrameworkCore;
using TailorAPI.DTO;

public class OrderService : IOrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly TailorDbContext _context;

    public OrderService(OrderRepository orderRepository, TailorDbContext context)
    {
        _orderRepository = orderRepository;
        _context = context;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllOrders();
        return orders.Select(o => new OrderResponseDto
        {
            CustomerName = _context.Customers.Find(o.CustomerID)?.FullName,
            ProductName = _context.Products.Find(o.ProductID)?.ProductName,
            Quantity = o.Quantity,
            TotalPrice = o.TotalPrice,
            OrderStatus = o.OrderStatus,
            PaymentStatus = o.PaymentStatus,
            OrderDate = o.OrderDate,
            CompletionDate = o.CompletionDate
        }).ToList();
    }

    public async Task<OrderResponseDto> GetOrderById(int id)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null) return null;
        return new OrderResponseDto
        {
            CustomerName = _context.Customers.Find(order.CustomerID)?.FullName,
            ProductName = _context.Products.Find(order.ProductID)?.ProductName,
            Quantity = order.Quantity,
            TotalPrice = order.TotalPrice,
            OrderStatus = order.OrderStatus,
            PaymentStatus = order.PaymentStatus,
            OrderDate = order.OrderDate,
            CompletionDate = order.CompletionDate
        };
    }

    public async Task<OrderResponseDto> CreateOrder(OrderResponseDto request)
    {
        // 🔹 Find Customer by Name
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.FullName == request.CustomerName);
        if (customer == null) throw new Exception("Customer not found");

        // 🔹 Find Product by Name
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == request.ProductName);
        if (product == null) throw new Exception("Product not found");

        // 🔹 Calculate Total Price
        var totalPrice = product.Price * request.Quantity;

        // 🔹 Create New Order
        var order = new Order
        {
            CustomerID = customer.CustomerID,  // ✅ Use ID found from Name
            ProductID = product.ProductID,    // ✅ Use ID found from Name
            Quantity = request.Quantity,
            TotalPrice = totalPrice,
            OrderStatus = request.OrderStatus,
            PaymentStatus = request.PaymentStatus,
            OrderDate = request.OrderDate,
            CompletionDate = request.CompletionDate
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // 🔹 Return OrderResponseDto with names instead of IDs
        return new OrderResponseDto
        {
            CustomerName = customer.FullName,  // ✅ Return Name instead of ID
            ProductName = product.ProductName,
            Quantity = order.Quantity,
            TotalPrice = order.TotalPrice,
            OrderStatus = order.OrderStatus,
            PaymentStatus = order.PaymentStatus,
            OrderDate = order.OrderDate,
            CompletionDate = order.CompletionDate
        };
    }



    public async Task<OrderResponseDto> UpdateOrder(int orderId, int quantity, string orderStatus, string paymentStatus, DateTime? completionDate)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null) return null;

        order.Quantity = quantity;
        order.OrderStatus = orderStatus;
        order.PaymentStatus = paymentStatus;
        order.CompletionDate = completionDate;

        await _orderRepository.UpdateOrder(order);
        return await GetOrderById(orderId);
    }

    public async Task<bool> DeleteOrder(int orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null) return false;

        await _orderRepository.DeleteOrder(order);
        return true;
    }
}
