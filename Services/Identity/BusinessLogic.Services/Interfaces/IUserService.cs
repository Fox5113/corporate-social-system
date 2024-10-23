using BusinessLogic.Models;

namespace BusinessLogic.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<bool> UpdateUserAsync(UserDto userDto);
    Task<bool> DeleteUserAsync(string userId);
}