using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterUserAsync(User user, string password)
    {
        try
        {
            return await _userManager.CreateAsync(user, password);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while registering the user", ex);
        }
    }

    public async Task<User> GetUserByIdAsync(string userName)
    {
        try
        {
            return await _userManager.FindByNameAsync(userName);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while fetching the user", ex);
        }
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        try
        {
            return await _userManager.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the user", ex);
        }
    }

    public async Task<IdentityResult> DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the user", ex);
        }
    }
}