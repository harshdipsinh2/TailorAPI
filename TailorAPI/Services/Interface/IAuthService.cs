using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;

namespace TailorAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<string?> AuthenticateUserAsync(string email, string password);
        string HashPassword(string password); // ✅ Add this method
        Task<IActionResult> RegisterUserAsync([FromBody] UserRequestDto request);

    }
}