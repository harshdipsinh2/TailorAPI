namespace TailorAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TailorAPI.Models;
    using System.Threading.Tasks;
    using TailorAPI.Services.Interface;
    using TailorAPI.DTO;

    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var newUser = await _userService.RegisterUserAsync(userDto);
            if (newUser == null)
                return BadRequest("User already exists");

            return Ok(newUser);
        }


        // Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userService.AuthenticateUserAsync(loginDto.Email, loginDto.Password);
            if (user == null) return Unauthorized("Invalid credentials");
            return Ok(user);
        }

        // Get all users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // Update user
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.UserID) return BadRequest("User ID mismatch");
            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }

        // Delete user
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            return success ? Ok("User deleted") : NotFound("User not found");
        }
    }
}

 
