using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestIdentity.Models;

namespace TestIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login request");

            var loginSuccess = await _authService.LoginAsync(loginDto);
            if (!loginSuccess)
            {
                return Unauthorized(new UserModel() { IsFound = false });
            }

            return RedirectToAction("GetUserByName", "Users", new { name = loginDto.UserName });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration request");

            var registerSuccess = await _authService.RegisterAsync(registerDto);
            if (!registerSuccess)
                return BadRequest("Registration failed");

            return RedirectToAction("SendMessageAndGetUserByName", "Users", new { name = registerDto.UserName });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok("Logout successful");
        }
    }
}