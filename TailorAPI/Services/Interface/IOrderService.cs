using TailorAPI.DTO.Request;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTOs.Request;

public interface IOrderService
{
    Task<OrderResponseDto> CreateOrderAsync(int customerId, int productId, int fabricTypeId, int assignedTo, OrderRequestDto request);
    Task<bool> UpdateOrderAsync(int id, int productId, int fabricTypeId, int assignedTo, OrderRequestDto request);

    Task<bool> UpdateOrderStatusAsync(int id, OrderStatusUpdateDto request);
    Task<bool> SoftDeleteOrderAsync(int id);

    Task<decimal> GetTotalRevenueAsync();
    Task<OrderResponseDto?> GetOrderByIdAsync(int id);
    public Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync(int userId, string role);
    Task<bool> UpdateOrderApprovalAsync(int orderId, OrderApprovalUpdateDTO RequestDTO);
}