using BusinessLogic.Models;

namespace BusinessLogic.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginRequestDto loginDto);
        Task<bool> RegisterAsync(RegisterUserDto registerDto);
        Task LogoutAsync();
    }
}