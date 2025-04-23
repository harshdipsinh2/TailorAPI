using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Repositories;
using TailorAPI.Services.Interface;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserRepository _userRepository;
    private readonly IOtpVerificationService _otpVerificationService;

    public AuthController(IAuthService authService, UserRepository userRepository, IOtpVerificationService otpVerificationService)
    {
        _authService = authService;
        _userRepository = userRepository;
        _otpVerificationService = otpVerificationService;
    }

    // Updated Login method to accept query parameters
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
    {
        var token = await _authService.AuthenticateUserAsync(email, password);
        /*if (token == null)
        {
            return Unauthorized();
        }*/

        return Ok(new { Token = token });
    }

    // Updated Register method to accept query parameters
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(
        [FromQuery] string name,
        [FromQuery] string email,
        [FromQuery] string password,
        [FromQuery] string mobileNo,
        [FromQuery] string address,
        [FromQuery] string roleName
    )
    {
        var request = new UserRequestDto
        {
            Name = name,
            Email = email,
            Password = password,
            MobileNo = mobileNo,
            Address = address,
            RoleName = roleName
        };

        var result = await _authService.RegisterUserAsync(request);

        if (result == null)
        {
            return Conflict(new { Message = "Email already exists or invalid role specified." });
        }

        return Ok(new { Message = "User registered successfully." });
    }
    [HttpPost("send")]
    public async Task<IActionResult> SendOtp(string email)
    {
        var result = await _otpVerificationService.GenerateAndSendOtpAsync(email);
        return result ? Ok("OTP sent.") : BadRequest("Failed to send OTP.");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationsRequestDTO dto)
    {
        var result = await _otpVerificationService.VerifyOtpAsync(dto);
        return result ? Ok("OTP verified.") : BadRequest("Invalid or expired OTP.");
    }
}
