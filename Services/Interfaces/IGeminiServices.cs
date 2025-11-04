using GuitarShop.Models;

namespace GuitarShop.Services.Interfaces
{
    public interface IGeminiServices
    {
        Task<string> ChatWithContextAsync(List<ChatMessage> history);
    }
}
