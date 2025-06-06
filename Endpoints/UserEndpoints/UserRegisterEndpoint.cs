﻿using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Services.IUserServices;
using CC_Karriarpartner.Services.UserServices;
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

                return result switch //switch that returns differents resuluts error or success
                {
                    RegistrationResult.Success => Results.Ok("User registered successfully"),
                    RegistrationResult.EmailAlreadyExists => Results.BadRequest("Email already exists"),
                    RegistrationResult.InvalidPassword => Results.BadRequest("Password does not meet requirements (must be at least 8 characters and include uppercase, lowercase, numbers, and special characters)"),
                    RegistrationResult.InvalidEmail => Results.BadRequest("Invalid email format"),
                    RegistrationResult.Error => Results.StatusCode(500),
                    _ => Results.StatusCode(500)
                };

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
