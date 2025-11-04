using GuitarShop.DTOs;
using GuitarShop.Models;
using GuitarShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuitarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetCart(int userId)
        {
            var cartItems = await _cartService.GetCartAsync(userId);
            return Ok(new { Success = true, Data = cartItems });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddCartItemRequest request)
        {
            await _cartService.AddToCartAsync(request.UserId, request.ProductId, request.Quantity);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            await _cartService.UpdateCartItemQuantityAsync(request.CartItemId, request.Quantity);
            return Ok();
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            await _cartService.RemoveCartItemAsync(cartItemId);
            return Ok();
        }
    }
}
