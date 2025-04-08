using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Services.IUserServices;
using CC_Karriarpartner.Services;
using Microsoft.EntityFrameworkCore;
using CC_Karriarpartner.Models;
using CC_Karriarpartner.DTOs.UserDtos;

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

        public async Task<List<UserPurchaseHistoryDto>> GetPurchaseHistory(int userId) //users history purchase
        {
            var purchases = await context.Purchases
                .Where(p => p.User.UserId == userId)
                .OrderByDescending(p => p.BuyDate)
                .Include(p => p.PurchaseItems)
                .ThenInclude(pi => pi.Course)
                .ToListAsync();
            var result = purchases.Select(p => new UserPurchaseHistoryDto
            {
                PurchaseId = p.PurchaseId,
                PurchaseDate = p.BuyDate,
                Price = p.Price,
                Items = p.PurchaseItems
              .Where(pi => pi.Course != null)
              .Select(pi => new UserPurchaseItemDto
              {
                  ItemId = pi.PurchaseItemId,
                  ProductName = pi.Course.Title,
                  ProductDescription = pi.Course.Description
              }).ToList()
            }).ToList();

            return result;
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

        public async Task<UserProfileDto> GetUserProfile(int userId)//get user profile info
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            return new UserProfileDto
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,

            };
        }

        public async Task<RegistrationResult> UpdateUserProfile(int userId, UpdateProfileDto profileDto)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return RegistrationResult.Error;

            if (string.IsNullOrWhiteSpace(profileDto.Name) ||
                string.IsNullOrWhiteSpace(profileDto.LastName) ||
                string.IsNullOrWhiteSpace(profileDto.Email))
            {
                return RegistrationResult.InvalidInput;
            }

            bool isEmailChanged = !string.IsNullOrEmpty(profileDto.Email) &&
                                  user.Email != profileDto.Email;



            if (isEmailChanged)
            {
                if (await context.Users.AnyAsync(user => user.Email == profileDto.Email))
                    return RegistrationResult.EmailAlreadyExists;

                string verificationToken = GenerateVerification();
                user.EmailVerification = verificationToken;
                user.Email = profileDto.Email;
                user.Verified = false;

                await emailService.SendVerificationEmailAsync(user.Email, verificationToken, user.Name);
            }
            user.Name = profileDto.Name;
            user.LastName = profileDto.LastName;
            user.Phone = profileDto.Phone;

            await context.SaveChangesAsync();
            return RegistrationResult.Success;
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
