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
        public UserRegisterService(KarriarPartnerDBContext _context)
        {
            context = _context;
        }

        public async Task<bool> RegisterUser(UserRegistrationDto userRegistrationDto)
        {
            if (await context.Users.AnyAsync(u => u.Email == userRegistrationDto.UserEmail))
            {
                return false; //checks already existing mail
            }

            string hashedPassword = PasswordHasher.HashPassword(userRegistrationDto.Password);

            var user = new User
            {
                Name = userRegistrationDto.UserName,
                LastName = userRegistrationDto.UserLastName,
                Email = userRegistrationDto.UserEmail,
                Phone = userRegistrationDto.UserPhone,
                Password = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                Verified = false
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return true;
        }


    }
}
