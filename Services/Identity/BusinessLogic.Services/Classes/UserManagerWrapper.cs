using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services.Classes;

public class UserManagerWrapper : IUserManagerWrapper
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagerWrapper(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<ApplicationUser?> FindByIdAsync(string userId) => _userManager.FindByIdAsync(userId);
    public Task<ApplicationUser?> FindByNameAsync(string userName) => _userManager.FindByNameAsync(userName);
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password) => _userManager.CreateAsync(user, password);
    public Task<IdentityResult> UpdateAsync(ApplicationUser user) => _userManager.UpdateAsync(user);
    public Task<IdentityResult> DeleteAsync(ApplicationUser user) => _userManager.DeleteAsync(user);
    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password) => _userManager.CheckPasswordAsync(user, password);

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
}