using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogic.Services.Classes
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return false;

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, false);
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

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result.Succeeded;
        }

        public async Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Login);
            var user2 = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || user?.Id != user2?.Id)//|| !(await _userManager.IsEmailConfirmedAsync(user))
            {
                return null;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return new ForgotPasswordResponseDto() { Id = user.Id, Code = code }; 
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var user2 = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || user?.Id != user2?.Id)// || !(await _userManager.IsEmailConfirmedAsync(user))
            {
                return null;
            }
            return await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}