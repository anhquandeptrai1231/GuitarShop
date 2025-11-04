using GuitarShop.DTOs;

namespace GuitarShop.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<string> RegisterAsync(string username, string password, string email);
        Task<LoginResultDTO?> LoginAsync(string username, string password);
    }
}
