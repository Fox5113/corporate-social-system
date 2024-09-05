using BusinessLogic.Dtos;
using DataAccess.Entities;
using DataAccess.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(string user, string password);
    Task<User> GetUserByIdAsync(string userId);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<IdentityResult> DeleteUserAsync(string userId);
    Task SignInAsync(UserDto userDto);
    Task SignOutAsync();
    Task<SignInResult> PasswordSignInAsync(UserRegistrationDto userDto);


}