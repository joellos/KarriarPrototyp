using CC_Karriarpartner.DTOs.AuthLogDto;
using CC_Karriarpartner.Services.AuthServices;

namespace CC_Karriarpartner.Endpoints.UserEndpoints
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
            });
        }
    }
}
