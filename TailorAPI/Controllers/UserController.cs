namespace TailorAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TailorAPI.Models;
    using System.Threading.Tasks;
    using TailorAPI.Services.Interface;
    using TailorAPI.DTO;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            var user = await _userService.RegisterUserAsync(userDto);
            if (user == null) return BadRequest("User registration failed.");

            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate(string email, string password)
        {
            var user = await _userService.AuthenticateUserAsync(email, password);
            if (user == null) return Unauthorized("Invalid credentials.");

            return Ok(user);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null) return NotFound("User not found.");

            return Ok("User updated successfully.");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound("User not found.");

            return Ok("User deleted successfully.");
        }
    }
}


 
