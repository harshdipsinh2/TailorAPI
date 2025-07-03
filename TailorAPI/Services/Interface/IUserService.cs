using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.DTO;

public interface IUserService
{

    Task<List<UserResponseDto>> GetAllTailorsAsync();
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> UpdateUserAsync(int id, UserRequestDto userDto);
    Task<UserResponseDto?> RegisterUserAsync(UserRequestDto userDto);
    Task<UserResponseDto?> AuthenticateUserAsync(string email, string password);  // Changed to DTO
    Task<List<UserResponseDto>> GetUsersForManager();
    Task<List<UserResponseDto>> GetUsersForAdmin(int shopId, int? branchId = null);
    Task<List<UserResponseDto>> GetUserForSuperAdmin(int shopId, int? branchId);
    Task<List<UserResponseDto>> GetAllUsersAsync();  // Changed to DTO
    Task<bool> DeleteUserAsync(int userId);
}
