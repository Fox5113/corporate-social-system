using BusinessLogic.Models;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services.Interfaces;

public interface IUserManagerWrapper
{
    Task<ApplicationUser?> FindByIdAsync(string userId);
    Task<ApplicationUser?> FindByNameAsync(string userName);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<IdentityResult> UpdateAsync(ApplicationUser user);
    Task<IdentityResult> DeleteAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordDto model);
    Task<IdentityResult> ResetPassword(ResetPasswordDto model);
}