using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Services.IUserServices;
using Microsoft.AspNetCore.Mvc;

namespace CC_Karriarpartner.Endpoints.UserEndpoints
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

                if (result)
                {
                    return Results.Ok("User registered successfully");
                }
                else
                {
                    return Results.BadRequest("Registration failed.");
                }
            });

            app.MapGet("/api/verify", async ([FromQuery] string email, [FromQuery] string token, IUserService userService) =>
            {
                var result = await userService.VerifyEmail(email, token);

                if (result)
                    return Results.Redirect("/verification-success.html"); // Redirect to a success page
                else
                    return Results.BadRequest("Invalid verification link");
            });


        }

    }
}
