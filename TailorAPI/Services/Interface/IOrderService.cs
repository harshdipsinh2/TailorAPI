using TailorAPI.DTO;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetAllOrders();
    Task<OrderResponseDto> GetOrderById(int id);
    Task<OrderResponseDto> CreateOrder(OrderResponseDto request);
    Task<OrderUpdateDto> UpdateOrder(int orderId, int quantity, string orderStatus, string paymentStatus, string completionDate);
    Task<bool> DeleteOrder(int orderId);
}