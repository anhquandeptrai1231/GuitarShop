using GuitarShop.DTOs;
using GuitarShop.Models;

namespace GuitarShop.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItemDTO>> GetCartAsync(int userId);
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task RemoveCartItemAsync(int cartItemId);
        Task UpdateCartItemQuantityAsync(int cartItemId, int quantity);
    }
}
