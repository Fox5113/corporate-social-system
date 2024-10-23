using DataAccess.Entities;

namespace DataAccess.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<IEnumerable<ApplicationUser?>> GetAllUsersAsync();
    Task<bool> CreateUserAsync(ApplicationUser? user, string password);
    Task<bool> UpdateUserAsync(ApplicationUser? user);
    Task<bool> DeleteUserAsync(string userId);
}