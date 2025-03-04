using TailorAPI.DTO;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetAllOrders();
    Task<OrderResponseDto> GetOrderById(int id);
    Task<OrderResponseDto> CreateOrder(OrderResponseDto request);
    Task<OrderResponseDto> UpdateOrder(int orderId, int quantity, string orderStatus, string paymentStatus, DateTime? completionDate);
    Task<bool> DeleteOrder(int orderId);
}