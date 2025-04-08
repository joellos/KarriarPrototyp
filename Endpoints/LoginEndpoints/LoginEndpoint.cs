using CC_Karriarpartner.DTOs.AuthLogDto;
using CC_Karriarpartner.Services.AuthServices;

namespace CC_Karriarpartner.Endpoints.LoginEndpoints
{
    public class LoginEndpoint
    {
        public static void LoginEndpointAsync(WebApplication app)
        {
            app.MapPost("/login", async (LoginDto loginDto, IAuthService authService) =>
            {
                if (loginDto == null)
                {
                    return Results.BadRequest("No data");
                }
                if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                {
                    return Results.BadRequest("Email and password required");
                }
                var result = await authService.LoginAsync(loginDto);
                if (result != null)
                {
                    return Results.Ok(result);
                }
                else
                {
                    return Results.BadRequest("Login failed.");
                }
            }).RequireRateLimiting("login");
            // Test endpoints
            app.MapGet("/AuthAdmin", () =>
            {
                return Results.Ok("Auth");

            }).RequireAuthorization("AdminPolicy");
            app.MapGet("/Auth", () =>
            {
                return Results.Ok("Logged in");

            }).RequireAuthorization();

        }
    }
}
