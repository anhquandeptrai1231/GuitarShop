using GuitarShop.Models;

namespace GuitarShop.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);

        Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);

        Task<CartItem?> GetCartItemAsync(int cartId, int productId);

        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);

        Task AddCartAsync(Cart cart);

        Task AddCartItemAsync(CartItem cartItem);

        Task RemoveCartItemAsync(CartItem cartItem);

        Task SaveChangesAsync();
    }
}
