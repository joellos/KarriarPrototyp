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

        public async Task<bool> RegisterUser(UserRegistrationDto userRegistrationDto)
        {
            if (await context.Users.AnyAsync(u => u.Email == userRegistrationDto.UserEmail))
            {
                return false; //checks already existing mail
            }

            string verificationToken = GenerateVerification();

            string hashedPassword = PasswordHasher.HashPassword(userRegistrationDto.Password);

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
            return true;
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


    }
}
