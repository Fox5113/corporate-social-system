using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using DataAccess.Entities;

namespace BusinessLogic.Services.Classes
{
    public class AuthService : IAuthService
    {
        private readonly ISignInManagerWrapper _signInManagerWrapper;
        private readonly IUserManagerWrapper _userManagerWrapper;

        public AuthService(ISignInManagerWrapper signInManagerWrapper, IUserManagerWrapper userManagerWrapper)
        {
            _signInManagerWrapper = signInManagerWrapper;
            _userManagerWrapper = userManagerWrapper;
        }

        public async Task<bool> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _userManagerWrapper.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return false;

            var result = await _signInManagerWrapper.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, false);
            return result.Succeeded;
        }

        public async Task<bool> RegisterAsync(RegisterUserDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Name = registerDto.Name
            };

            var result = await _userManagerWrapper.CreateAsync(user, registerDto.Password);
            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManagerWrapper.SignOutAsync();
        }
    }
}