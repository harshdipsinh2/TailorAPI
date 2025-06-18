using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.Services.Interface;

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
    public async Task<IActionResult> RegisterUser([FromBody] UserRequestDto userrequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _userService.RegisterUserAsync(userrequestDto);
        if (user == null) return BadRequest("User registration failed.");

        return Ok(user);
    }

    [HttpGet("GetAll")]

    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("GetById/{id}")]

    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound("User not found.");

        return Ok(user);
    }

    [HttpDelete("Delete/{id}")]

    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound("User not found.");

        return Ok("User deleted successfully.");
    }

    [HttpPut("Update/{id}")]

    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDto userDto)
    {
        var updatedUser = await _userService.UpdateUserAsync(id, userDto);
        if (updatedUser == null) return NotFound("User not found.");

        return Ok(updatedUser);
    }
}
