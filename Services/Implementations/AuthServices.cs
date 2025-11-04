using GuitarShop.Data;
using GuitarShop.DTOs;
using GuitarShop.Models;
using GuitarShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GuitarShop.Services.Implementations
{
    public class AuthServices : IAuthServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthServices(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<LoginResultDTO?> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

           
            var token = GenerateJwtToken(user); 

            return new LoginResultDTO
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username
            };
        }

        public async Task<string> RegisterAsync(string username, string password, string email)
        {
         
            if (_context.Users.Any(u => u.Username == username))
                throw new Exception("Username already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Email = email,
                Role = "User",
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully";
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
