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
        /// <summary>
        /// Handles user login by validating credentials and generating tokens
        /// </summary>
        /// <param name="request">Login request containing email and password</param>
        /// <returns>Token response with access and refresh tokens, or null if authentication fails</returns>
        public async Task<TokenResponseDto> LoginAsync(LoginDto request)
        {
            // Find user by email in the database
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            // Return null if user was not found
            if (user is null)
            {
                return null;
            }

            // Verify that the provided password matches the stored password hash
            if (!PasswordHasher.VerifyPassword(request.Password, user.Password))
            {
                return null;
            }

            // Generate tokens and create response after successful authentication
            TokenResponseDto response = await CreateTokenResponse(user);
            return response;
        }

        /// <summary>
        /// Generates a new refresh token and saves it to the user record
        /// </summary>
        /// <param name="user">User to generate and save refresh token for</param>
        /// <returns>The generated refresh token string</returns>
        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            // Generate a new random refresh token
            var refreshToken = RefreshTokenGenerator();

            // Update user's refresh token and expiration time (7 days from now)
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);

            // Save changes to the database
            await context.SaveChangesAsync();

            return refreshToken;
        }

        /// <summary>
        /// Generates a cryptographically secure random string for use as a refresh token
        /// </summary>
        /// <returns>Base64 encoded random string</returns>
        private static string RefreshTokenGenerator()
        {
            // Create a byte array for storing random data
            var randomNum = new byte[32];

            // Use cryptographically secure random number generator
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNum);

            // Convert random bytes to Base64 string for storage and transmission
            return Convert.ToBase64String(randomNum);
        }

        /// <summary>
        /// Validates a refresh token for a specific user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="refreshToken">Refresh token to validate</param>
        /// <returns>User object if token is valid, null otherwise</returns>
        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            // Find user by ID
            var user = await context.Users.FindAsync(userId);

            // Return null if any of the following conditions are true:
            // - User not found
            // - Stored refresh token doesn't match provided token
            // - Refresh token has expired
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime < DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Refreshes access and refresh tokens for a user
        /// </summary>
        /// <param name="request">Request containing user ID and current refresh token</param>
        /// <returns>New token response or null if refresh token is invalid</returns>
        public async Task<TokenResponseDto?> RefreshTokensAsync(RequestRefreshTokenDto request)
        {
            // Validate the provided refresh token
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

            // Return null if validation fails
            if (user is null)
            {
                return null;
            }

            // Generate new tokens after successful validation
            return await CreateTokenResponse(user);
        }

        /// <summary>
        /// Creates a token response containing new access and refresh tokens
        /// </summary>
        /// <param name="user">User to create tokens for</param>
        /// <returns>Token response with access and refresh tokens</returns>
        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        /// <summary>
        /// Creates a JWT access token for the user
        /// </summary>
        /// <param name="_user">User to create token for</param>
        /// <returns>JWT token string</returns>
        private string CreateToken(User _user)
        {
            // Create the claims for the token containing the user's information
            var claims = new List<Claim>
            {
                // To uniquely identify the user without relying on information that can be changed
                new(ClaimTypes.NameIdentifier, _user.UserId.ToString()),
                new(ClaimTypes.Name, _user.Name),
                new(ClaimTypes.Surname, _user.LastName),
                new(ClaimTypes.Email, _user.Email),
                // Add roles here if implementing role-based authorization
            };

            // Create a key to sign the token with a secret key using HMACSHA512
            // ! is used to tell the compiler that the value is not null
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("Appsettings:Token")!));

            // Create signing credentials using the key (needs 64 characters for HMACSHA512)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            // Create the token descriptor containing the claims, the expiry date, and the signing credentials
            var tokenDescriptor = new JwtSecurityToken(
                // Issuer of the token (your application)
                issuer: configuration.GetValue<string>("Appsettings:Issuer"),
                // Intended recipient of the token (your application or a specific part of it)
                audience: configuration.GetValue<string>("Appsettings:Audience"),
                // Claims containing user information
                claims: claims,
                // Token expiration time (1 hour to reduce the window of attack if the token is stolen)
                expires: DateTime.UtcNow.AddHours(1),
                // Credentials for signing the token
                signingCredentials: creds
            );

            // Create and return the token using the JWT security token handler
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}