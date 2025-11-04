using GuitarShop.DTOs;
using GuitarShop.Models;
using GuitarShop.Repository.Interfaces;
using GuitarShop.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuitarShop.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        // Lấy danh sách CartItem theo userId
        public async Task<List<CartItemDTO>> GetCartAsync(int userId)
        {
            var items = await _repository.GetCartItemsByUserIdAsync(userId);

            return items.Select(ci => new CartItemDTO
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                ProductImage = ci.Product.ImageUrl,
                ProductPrice = (double)ci.Product.Price,
                Quantity = ci.Quantity
            }).ToList();
        }

        // Thêm sản phẩm vào giỏ hàng
        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await _repository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _repository.AddCartAsync(cart);
                await _repository.SaveChangesAsync(); // cần để có Cart.Id
            }

            var cartItem = await _repository.GetCartItemAsync(cart.Id, productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _repository.AddCartItemAsync(cartItem);
            }

            await _repository.SaveChangesAsync();
        }

        // Xoá item khỏi giỏ
        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _repository.GetCartItemByIdAsync(cartItemId);
            if (cartItem != null)
            {
                await _repository.RemoveCartItemAsync(cartItem);
                await _repository.SaveChangesAsync();
            }
        }

        // Cập nhật số lượng sản phẩm trong giỏ
        public async Task UpdateCartItemQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _repository.GetCartItemByIdAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _repository.SaveChangesAsync();
            }
        }
    }
}
