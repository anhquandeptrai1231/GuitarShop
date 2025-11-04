using GuitarShop.Data;
using GuitarShop.DTOs;
using GuitarShop.Models;
using GuitarShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GuitarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;

        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request.Username, request.Password, request.Email);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var result = await _authService.LoginAsync(request.Username, request.Password);

            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new
            {
                token = result.Token,
                userId = result.UserId,
                username = result.Username
            });
        }
    }
}
