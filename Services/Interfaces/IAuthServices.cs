namespace GuitarShop.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<string> RegisterAsync(string username, string password, string email);
        Task<string?> LoginAsync(string username, string password);
    }
}
