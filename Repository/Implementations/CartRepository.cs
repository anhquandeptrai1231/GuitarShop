using GuitarShop.Data;
using GuitarShop.Models;
using GuitarShop.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GuitarShop.Repository.Implementations
{
    public class CartRepository : ICartRepository
    {
        
            private readonly AppDbContext _context;

            public CartRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<Cart?> GetCartByUserIdAsync(int userId)
            {
                return await _context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                return new List<CartItem>();

            return await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cart.Id)
                .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
            {
                return await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
            }

            public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
            {
                return await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            }

            public async Task AddCartAsync(Cart cart)
            {
                await _context.Carts.AddAsync(cart);
            }

            public async Task AddCartItemAsync(CartItem cartItem)
            {
                await _context.CartItems.AddAsync(cartItem);
            }

            public async Task RemoveCartItemAsync(CartItem cartItem)
            {
                _context.CartItems.Remove(cartItem);
            }

            public async Task SaveChangesAsync()
            {
                await _context.SaveChangesAsync();
            }
        }
    }
