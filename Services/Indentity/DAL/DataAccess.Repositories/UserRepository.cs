using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DataAccess.Context;
using DataAccess.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthContext _context;
    private readonly UserManager<User> _userManager;

    public UserRepository(AuthContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteUserAsync(User user)
    {
        return await _userManager.DeleteAsync(user);
    }
}