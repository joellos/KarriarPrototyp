using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Services.IUserServices;
using CC_Karriarpartner.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using CC_Karriarpartner.DTOs.UserDtos;
using CC_Karriarpartner.DTOs.PasswordDtos;

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

            }).WithTags("Login and Register");

            app.MapGet("/api/verify", async ([FromQuery] string email, [FromQuery] string token, IUserService userService) =>
            {
                var result = await userService.VerifyEmail(email, token);

                if (result)
                    return Results.Redirect("/verification-success.html"); // Redirect to a success page
                else
                    return Results.BadRequest("Invalid verification link");
            }).WithTags("Login and Register");

            app.MapGet("/api/user/purchases", [Authorize] async (ClaimsPrincipal user, IUserService userService) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Results.BadRequest("Could not identify user");
                }

                var purchaseHistory = await userService.GetPurchaseHistory(userId);
                if (purchaseHistory.Count == 0)
                {
                    return Results.NoContent();
                }

                return Results.Ok(purchaseHistory);
            }).WithTags("User Profile");

            app.MapGet("/api/user/profile", [Authorize] async (ClaimsPrincipal user, IUserService service) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Results.BadRequest("Could not identify user");
                }
                var profile = await service.GetUserProfile(userId);
                if (profile == null)
                    return Results.NotFound("User not found");

                return Results.Ok(profile);

            }).WithName("GetUserProfile")
            .WithDescription("Get specefic user profile")
            .WithTags("User Profile");

            app.MapPut("/api/user/profile", [Authorize] async (ClaimsPrincipal user, IUserService service, UpdateProfileDto profileDto) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Results.BadRequest("Could not identify user");
                }
                var result = await service.UpdateUserProfile(userId, profileDto);

                return result switch
                {
                    RegistrationResult.Success => Results.Ok(new { message = "Profile updated successfully" }),
                    RegistrationResult.EmailAlreadyExists => Results.BadRequest("Email already in use"),
                    RegistrationResult.InvalidEmail => Results.BadRequest("Invalid email format"),
                    RegistrationResult.InvalidName => Results.BadRequest("Name and last name are required"),
                    RegistrationResult.InvalidInput => Results.BadRequest("Name, last name, and email are required fields and cannot be empty"),
                    _ => Results.Problem("Failed to update profile")
                };
            })
                .WithDisplayName("UpdateUserProfile")
                .WithDescription("Update personal information as email, name etc for a specific logged in user ")
                .WithTags("User Profile");

            app.MapPost("/api/forgot-password", async (ForgotPasswordDto model, IUserService userService) =>
            {
                if (string.IsNullOrEmpty(model.Email))
                {
                    return Results.BadRequest("Email is required");
                }

                await userService.RequestPasswordResetAsync(model.Email);

                // Always return success to prevent email enumeration attacks
                return Results.Ok(new { message = "If the email exists, a reset link has been sent." });
            }).WithTags("User Profile");

            app.MapGet("/reset-password", async (HttpContext context, string email, string token) =>
            {
                // Validate token and email
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                {
                    return Results.Redirect("/invalid-reset.html");
                }

                // Redirect to the HTML page
                return Results.Redirect($"/reset-password.html?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");
            }).WithTags("User Profile");

            app.MapPost("/api/reset-password", async (ResetPasswordDto resetDto, IUserService userService) =>
            {
                if (resetDto == null)
                {
                    return Results.BadRequest(new { message = "Invalid request data" });
                }

                var result = await userService.ResetPasswordAsync(resetDto);

                if (result)
                {
                    return Results.Ok(new { message = "Password has been reset successfully" });
                }

                return Results.BadRequest(new { message = "Invalid or expired password reset token" });
            }).WithTags("User Profile");

            app.MapDelete("/api/user/profile", [Authorize] async (ClaimsPrincipal user, [FromBody] DeleteUserDto deleteDto, IUserService userService, ILogger<Program> logger) =>
            {
                try
                {
                    if (deleteDto == null)
                    {
                        logger.LogWarning("DeleteUserDto is null");
                        return Results.BadRequest("No password provided");
                    }

                    logger.LogInformation("Attempting to delete user profile");

                    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                    {
                        logger.LogWarning("Failed to extract user ID from claims");
                        return Results.BadRequest("Could not identify user");
                    }

                    var result = await userService.DeleteUserProfile(userId, deleteDto.Password);

                    if (result)
                    {
                        logger.LogInformation($"User {userId} deleted successfully");
                        return Results.Ok(new { message = "Profile deleted successfully" });
                    }
                    else
                    {
                        logger.LogWarning($"Failed to delete user {userId} - password verification failed");
                        return Results.BadRequest("Failed to delete profile. Please check your password.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error deleting user profile");
                    return Results.Problem("An error occurred while deleting profile");
                }
            }).WithName("DeleteUserProfile")
            .WithTags("User Profile");





        }
    }
}
