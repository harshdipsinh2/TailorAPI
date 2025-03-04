using TailorAPI.DTO;

public interface IUserService
{
    Task<User> RegisterUserAsync(UserDto userDto); // Accept DTO instead of User

    Task<User?> AuthenticateUserAsync(string email, string password);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int userId);
}
