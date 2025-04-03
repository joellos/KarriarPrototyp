using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Services.IUserServices;
using CC_Karriarpartner.Services;
using Microsoft.EntityFrameworkCore;
using CC_Karriarpartner.Models;

namespace CC_Karriarpartner.Services.UserServices
{
    public class UserRegisterService : IUserService
    {
        private readonly KarriarPartnerDBContext context;
        private readonly IEmailService emailService;
        public UserRegisterService(KarriarPartnerDBContext _context, IEmailService _emailService)
        {
            context = _context;
            emailService = _emailService;
        }

        public async Task<RegistrationResult> RegisterUser(UserRegistrationDto userRegistrationDto)
        {
            if (!IsValidEmail(userRegistrationDto.UserEmail))
            {
                return RegistrationResult.InvalidEmail;
            }

            if (await context.Users.AnyAsync(u => u.Email == userRegistrationDto.UserEmail))
            {
                return RegistrationResult.EmailAlreadyExists; //checks already existing mail
            }


            if (!IsPasswordValid(userRegistrationDto.Password))
            {
                return RegistrationResult.InvalidPassword; // Password doesn't meet the requirements
            }

            string verificationToken = GenerateVerification();

            string hashedPassword = PasswordHasher.HashPassword(userRegistrationDto.Password);

            try
            {
                var user = new User
                {
                    Name = userRegistrationDto.UserName,
                    LastName = userRegistrationDto.UserLastName,
                    Email = userRegistrationDto.UserEmail,
                    Phone = userRegistrationDto.UserPhone,
                    Password = hashedPassword,
                    CreatedAt = DateTime.UtcNow,
                    Verified = false,
                    EmailVerification = verificationToken
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                await emailService.SendVerificationEmailAsync(user.Email, verificationToken, user.Name);
                return RegistrationResult.Success;

            }
            catch
            {
                return RegistrationResult.Error;

            }

        }

        public async Task<bool> VerifyEmail(string email, string token)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == email && u.EmailVerification == token);

            if (user == null)
            {
                return false;
            }
            user.Verified = true;
            user.EmailVerification = null;


            await context.SaveChangesAsync();
            return true;
        }

        private string GenerateVerification()
        {
            return Guid.NewGuid().ToString("N");
        }
        private static bool IsPasswordValid(string password)
        {
            bool result = password.Length >= 8 &&
                          password.Any(char.IsLower) &&
                          password.Any(char.IsUpper) &&
                          password.Any(char.IsDigit) &&
                         password.Any(ch => !char.IsLetterOrDigit(ch));
            return result;
        }
        private bool IsValidEmail(string email)//validates email
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);//validates the email with help of .net 
                return addr.Address == email;
            }
            catch
            {
                return false;
            }


        }
    }
}
