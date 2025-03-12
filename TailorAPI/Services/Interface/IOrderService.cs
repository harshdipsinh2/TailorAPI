using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;

public interface IOrderService
{
    Task<OrderResponseDto> CreateOrderAsync(int customerId, int productId, int fabricId, OrderRequestDto request);
    Task<bool> UpdateOrderAsync(int id, int productId, int fabricId, OrderRequestDto request);
    Task<bool> SoftDeleteOrderAsync(int id);
    Task<OrderResponseDto?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
}