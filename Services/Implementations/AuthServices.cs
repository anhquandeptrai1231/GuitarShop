using GuitarShop.Data;
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
        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return null;

            // Kiểm tra password
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid) return null;

            // Sinh JWT token
            return GenerateJwtToken(user);
        }

        public async Task<string> RegisterAsync(string username, string password, string email)
        {
            // Kiểm tra username tồn tại
            if (_context.Users.Any(u => u.Username == username))
                throw new Exception("Username already exists");

            // Hash mật khẩu
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
