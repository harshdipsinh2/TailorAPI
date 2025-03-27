using TailorAPI.DTOs.Request;

public interface IOrderService
{
    Task<OrderResponseDto> CreateOrderAsync(int customerId, int productId, int fabricTypeId, int assignedTo, OrderRequestDto request);
    Task<bool> UpdateOrderAsync(int id, int productId, int fabricTypeId, int assignedTo, OrderRequestDto request);

    Task<bool> SoftDeleteOrderAsync(int id);
    Task<OrderResponseDto?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
}