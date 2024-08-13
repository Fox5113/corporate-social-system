using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(User user, string password);
    Task<User> GetUserByIdAsync(string userId);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<IdentityResult> DeleteUserAsync(string userId);
}