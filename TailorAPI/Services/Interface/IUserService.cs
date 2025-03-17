using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.DTO;

public interface IUserService
{
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> UpdateUserAsync(int id, UserRequestDto userDto);
    Task<UserResponseDto?> RegisterUserAsync(UserRequestDto userDto);
    Task<UserResponseDto?> AuthenticateUserAsync(string email, string password);  // Changed to DTO
    Task<List<UserResponseDto>> GetAllUsersAsync();  // Changed to DTO
    Task<bool> DeleteUserAsync(int userId);
}
