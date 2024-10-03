using System.Threading.Tasks;
using DataAccess.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(string userId);
    Task<IdentityResult> CreateUserAsync(User user, string password);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<IdentityResult> DeleteUserAsync(User user);
}