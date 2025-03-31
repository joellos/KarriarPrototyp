using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Services.IUserServices;

namespace CC_Karriarpartner.Endpoints
{
    public class UserRegisterEndpoint
    {
        public static void RegisterUserEndpoints(WebApplication app)
        {
            app.MapPost("/register", async (UserRegistrationDto userDto, IUserService userService) =>
            {
                if (userDto == null)
                {
                    return Results.BadRequest("No data");
                }

                if (string.IsNullOrEmpty(userDto.UserEmail) || string.IsNullOrEmpty(userDto.Password))
                {
                    return Results.BadRequest("Email and password required");
                }

                var result = await userService.RegisterUser(userDto);

                if(result)
                {
                    return Results.Ok("User registered successfully");
                }
                else
                {
                    return Results.BadRequest("Registration failed.");
                }
            });
        }
    }
}
