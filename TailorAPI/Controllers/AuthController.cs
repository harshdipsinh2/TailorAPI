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

    // ✅ Login with Email & Password
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
    {
        var token = await _authService.AuthenticateUserAsync(email, password);
        if (token == null)
        {
            return Unauthorized("Invalid credentials or account not verified.");
        }

        return Ok(new { Token = token });
    }

    // ✅ Updated Registration: Sends OTP to email
    //[HttpPost("register")]
    //public async Task<IActionResult> RegisterUserAsync([FromBody] UserRequestDto request)
    //{
    //    var result = await _authService.RegisterUserWithOtpAsync(request);
    //    return result;
    //}

    // ✅ OTP Verification Endpoint (email + otp)
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtpAndActivate([FromBody] OtpVerificationsRequestDTO dto)
    {
        var verified = await _authService.VerifyOtpAndActivateUserAsync(dto);
        return verified
            ? Ok("OTP verified and user activated.")
            : BadRequest("Invalid or expired OTP.");
    }

    [HttpPost("register/admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegistrationDto request)
    {
        return await _authService.RegisterAdminAsync(request);
    }

    [HttpPost("register/employee")]
    public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegistrationDto request)
    {
        return await _authService.RegisterEmployeeAsync(request);
    }



    // ❌ [Deprecated] Old OTP Send Endpoint (can remove)
    //[HttpPost("send")]
    //public async Task<IActionResult> SendOtp(string email)
    //{
    //    var result = await _otpVerificationService.GenerateAndSendOtpAsync(email);
    //    return result ? Ok("OTP sent.") : BadRequest("Failed to send OTP.");
    //}

    // ❌ [Deprecated] Old OTP Verify Endpoint (can remove)
    //[HttpPost("verify")]
    //public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationsRequestDTO dto)
    //{
    //    var result = await _otpVerificationService.VerifyOtpAsync(dto);
    //    return result ? Ok("OTP verified.") : BadRequest("Invalid or expired OTP.");
    //}
}
