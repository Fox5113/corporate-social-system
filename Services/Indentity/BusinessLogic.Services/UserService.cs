using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using BusinessLogic.Dtos;
using DataAccess.Entities.Entities;

namespace BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<User> _signInManager;

    public UserService(SignInManager<User> signInManager, IUserRepository userRepository)
    {
        _signInManager = signInManager;
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> RegisterUserAsync(string userName, string password)
    {
        var user = new User { UserName = userName };
        return await _userRepository.CreateUserAsync(user, password);
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<IdentityResult> DeleteUserAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user != null)
        {
            return await _userRepository.DeleteUserAsync(user);
        }
        else
        {
            throw new Exception("User not found");
        }
    }

    public async Task SignInAsync(UserDto userDto)
    {
        var user = new User { UserName = userDto.UserName };
        await _signInManager.SignInAsync(user, false);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<SignInResult> PasswordSignInAsync(UserRegistrationDto userDto)
    {
        return await _signInManager.PasswordSignInAsync(userDto.UserName, userDto.Password, false, false);
    }
}