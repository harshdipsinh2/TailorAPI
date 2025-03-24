using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.Repositories;
using TailorAPI.Services.Interface;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService; // ✅ Depends on interface
    private readonly UserRepository _userRepository;

    public AuthController(IAuthService authService, UserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var token = await _authService.AuthenticateUserAsync(email, password);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRequestDto userDto)
    {
        var user = new User
        {
            Email = userDto.Email,
            PasswordHash = _authService.HashPassword(userDto.Password),
            // other user properties
        };

        await _userRepository.CreateUserAsync(user);
        return Ok();
    }
}
