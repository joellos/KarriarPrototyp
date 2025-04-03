using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs.AuthLogDto;
using CC_Karriarpartner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CC_Karriarpartner.Services.AuthServices
{
    public class AuthService(KarriarPartnerDBContext context, IConfiguration configuration) : IAuthService
    {

        public async Task<TokenResponseDto> LoginAsync(LoginDto request)
        {
            // Login method for user authentication and generating JWT tokens
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null || !user.Verified)
            {
                return null;
            }

            if (!PasswordHasher.VerifyPassword(request.Password, user.Password))
            {
                return null;
            }

            TokenResponseDto response = await CreateTokenResponse(user);
            return response;
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            // Generate a new refresh token and save it to the database
            var refreshToken = RefreshTokenGenerator();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);

            await context.SaveChangesAsync();

            return refreshToken;
        }

        private static string RefreshTokenGenerator()
        {
            // Generate a cryptographically secure token with 256 bits (32 bytes) of entropy
            // This provides protection against brute force and prediction attacks
            var randomNum = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNum);
            return Convert.ToBase64String(randomNum);
        }

        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {


            var user = await context.Users.FindAsync(userId);

            // Security check: All conditions must be met for a valid refresh token
            // 1. User must exist
            // 2. Provided token must match stored token (prevents token forgery)
            // 3. Token must not be expired (prevents use of old compromised tokens)
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime < DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }
        public async Task<TokenResponseDto?> RefreshTokensAsync(RequestRefreshTokenDto request)
        {
            // Validate the refresh token and user ID 
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

            if (user is null)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            // Generate a new access token and refresh token
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        private string CreateToken(User _user)
        {
            // Build essential user claims for the JWT
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, _user.UserId.ToString()),
                new(ClaimTypes.Name, _user.Name),
                new(ClaimTypes.Surname, _user.LastName),
                new(ClaimTypes.Email, _user.Email),
                new(ClaimTypes.Role, _user.Role)
            };

            // Create signing key from app settings
            // HMACSHA512 requires at least 64 bytes for security
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("Appsettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            // Configure token with 20-minute expiration for security
            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Appsettings:Issuer"),
                audience: configuration.GetValue<string>("Appsettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}