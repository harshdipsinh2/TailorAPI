using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.DTO.RequestDTO;

namespace TailorAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<string?> AuthenticateUserAsync(string email, string password);
        string HashPassword(string password); // ✅ Add this method
        Task<IActionResult> RegisterUserAsync([FromBody] UserRequestDto request);
        Task<IActionResult> RegisterUserWithOtpAsync(UserRequestDto request);
        Task<bool> VerifyOtpAndActivateUserAsync(OtpVerificationsRequestDTO dto);

    }
}