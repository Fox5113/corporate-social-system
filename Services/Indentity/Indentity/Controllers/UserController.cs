using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Indentity.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, SignInManager<User> signInManager,
            IIdentityServerInteractionService interactionService, ITokenService tokenService, IConfiguration configuration)
        {
            _userService = userService;
            _signInManager = signInManager;
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

            var user = new User
            {
                UserName = vm.UserName
            };

            var result = await _userService.RegisterUserAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok(vm);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return Unauthorized();
            }
    
            var user = await _userService.GetUserByIdAsync(vm.UserName);
            var secret = _configuration.GetSection("secret").Value;
            
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Id, user.Id.ToString())
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
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(id);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}
