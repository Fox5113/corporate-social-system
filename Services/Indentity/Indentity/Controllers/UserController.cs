using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using DataAccess.Entities.Entities;
using IdentityModel;
using IdentityServer4.Services;
using Indentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Indentity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, SignInManager<User> signInManager,
            IIdentityServerInteractionService interactionService, ITokenService tokenService, IConfiguration configuration)
        {
            _userService = userService;
            _interactionService = interactionService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new UserDto
            {
                UserName = vm.UserName
            };

            var result = await _userService.RegisterUserAsync(vm.UserName, vm.Password);

            if (result.Succeeded)
            {
                await _userService.SignInAsync(user);
                return Ok(vm);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRegistrationViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userRegistrationDto = new UserRegistrationDto
            {
                UserName = vm.UserName,
                Password = vm.Password
            };
            var result = await _userService.PasswordSignInAsync(userRegistrationDto);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return Unauthorized();
            }
            
            var secret = _configuration.GetSection("secret").Value;
            
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, userRegistrationDto.UserName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret??"")), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = Guid.Parse(user.Id),
                UserName = user.UserName
            };

            return Ok(userDto);
        }
        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(string id)
        {
            await _userService.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(id);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}
