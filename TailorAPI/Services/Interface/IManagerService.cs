using TailorAPI.DTOs.Request;

public interface IManagerService
{

    Task<OrderResponseDto?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
}