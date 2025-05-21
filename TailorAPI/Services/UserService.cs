using Microsoft.AspNetCore.Identity;
using TailorAPI.DTO;
using TailorAPI.Repositories;
using TailorAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TailorAPI.Services.Interface;


public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly JwtService _JwtService;

    public UserService(UserRepository userRepository,JwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
        _JwtService = jwtService;
    }

  


    public async Task<UserResponseDto?> RegisterUserAsync(UserRequestDto userDto)
    {
        var users = await _userRepository.GetAllUsersAsync();
        if (users.Any(u => u.Email == userDto.Email)) return null;

        var role = await _userRepository.GetRoleByNameAsync(userDto.RoleName);
        if (role == null) return null;

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            MobileNo = userDto.MobileNo,
            Address = userDto.Address,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            RoleID = role.RoleID
        };

        await _userRepository.CreateUserAsync(user);

        return new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            MobileNo = user.MobileNo,
            Address = user.Address,
            RoleName = role.RoleName,
            UserStatus = user.UserStatus.ToString(),
            IsVerified = false
        };
    }

    public async Task<UserResponseDto?> AuthenticateUserAsync(string email, string password)
    {
        var user = (await _userRepository.GetAllUsersAsync())
            .FirstOrDefault(u => u.Email == email);

        if (user == null || user.IsDeleted) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result != PasswordVerificationResult.Success) return null;

        return new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            MobileNo = user.MobileNo,
            Address = user.Address,
            RoleName = user.Role.RoleName,
            UserStatus = user.UserStatus.ToString(),
            
        };
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(user => new UserResponseDto
        {
            UserID = user.UserID,
            Name = user.Name,
            Email = user.Email,
            MobileNo = user.MobileNo,
            Address = user.Address,
            RoleName = user.Role.RoleName,
            UserStatus = user.UserStatus.ToString()
        }).ToList();
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        return await _userRepository.DeleteUserAsync(userId);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return null;

        return new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            MobileNo = user.MobileNo,
            Address = user.Address,
            RoleName = user.Role.RoleName,
            UserStatus = user.UserStatus.ToString()
        };
    }

    public async Task<UserResponseDto?> UpdateUserAsync(int id, UserRequestDto userDto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null) return null;

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;
        existingUser.MobileNo = userDto.MobileNo;
        existingUser.Address = userDto.Address;

        await _userRepository.UpdateUserAsync(existingUser);

        return new UserResponseDto
        {
            Name = existingUser.Name,
            Email = existingUser.Email,
            MobileNo = existingUser.MobileNo,
            Address = existingUser.Address,
            RoleName = existingUser.Role.RoleName,
            UserStatus = existingUser.UserStatus.ToString()
        };
    }
}
