using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs.AuthLogDto;
using CC_Karriarpartner.Models;
using CC_Karriarpartner.Services;
using CC_Karriarpartner.Services.AuthServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CC_Karriarpartner.Endpoints.LoginEndpoints
{
    public class LoginEndpoint
    {
        public static void LoginEndpointAsync(WebApplication app)
        {
            app.MapPost("/login", async (LoginDto loginDto, IAuthService authService, HttpContext httpContext) =>
            {
                if (loginDto == null)
                {
                    return Results.BadRequest("No data");
                }
                if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                {
                    return Results.BadRequest("Email and password required");
                }
                var result = await authService.AuthenticateUserAsync(loginDto);

                if (result is null)
                {
                    return Results.BadRequest("Invalid credentials");
                }
                // Set the access token cookie
                httpContext.Response.Cookies.Append("accessToken", result.AccessToken, GetCookieOptions.AccessTokenCookie());
                httpContext.Response.Cookies.Append("refreshToken", result.RefreshToken, GetCookieOptions.RefreshTokenCookie());

                return Results.Ok(result); // return the token response 

                // ALTERNATIVE if we want to restun user data/infio to frontend, Need getUserByIdAsync method in the service
                //return Results.Ok(new
                //{
                //    userId = user.UserId.ToString(),
                //    username = user.UserName,
                //    email = user.Email
                //});


            }).WithTags("Login and Register")
                .RequireRateLimiting("login"); // limit login attempts

            // endpoint to refresh the token
            app.MapPost("/refresh-token", async (IAuthService service, KarriarPartnerDBContext context, HttpContext http) =>
            {
                // Check if refresh token exists in cookies
                if (!http.Request.Cookies.TryGetValue("refreshToken", out string? refreshToken))
                {
                    return Results.Unauthorized();
                }

                // Find user with this refresh token
                var user = await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
                if (user == null || user.RefreshTokenExpireTime <= DateTime.UtcNow)
                {
                    return Results.Unauthorized();
                }

                // Generate new token pair
                var accessToken = service.GenerateAccessToken(user);
                var newRefreshToken = service.GenerateSecureRefreshToken(); // Make this public

                // Update user in database
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
                await context.SaveChangesAsync();

                // Set cookies
                http.Response.Cookies.Append("accessToken", accessToken, GetCookieOptions.AccessTokenCookie());

                http.Response.Cookies.Append("refreshToken", newRefreshToken, GetCookieOptions.RefreshTokenCookie());

                return Results.Ok();

            }).AllowAnonymous()
            .WithTags("Login and Register");
            // allow anonymous because if the accestoken is expired, we need to allow the user to get a new one
            
            // endpoint to logout
            app.MapPost("/logout", (HttpContext context) =>
            {
                context.Response.Cookies.Delete("accessToken", GetCookieOptions.AccessTokenCookie());
                context.Response.Cookies.Delete("refreshToken", GetCookieOptions.RefreshTokenCookie());


                return Results.Ok("Logged out successfully");

            }).RequireAuthorization().WithTags("Login and Register");

            // Test endpoints
            app.MapGet("/AuthAdmin", () =>
            {
                return Results.Ok("Auth");

            }).WithTags("Login and Register")
                .RequireAuthorization("AdminPolicy");
            app.MapGet("/Auth", () =>
            {
                return Results.Ok("Logged in");

            }).RequireAuthorization().WithTags("Login and Register");
        }
    }
}
