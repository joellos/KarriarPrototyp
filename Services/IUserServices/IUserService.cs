using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.Services.UserServices;

namespace CC_Karriarpartner.Services.IUserServices
{
    public interface IUserService
    {
        Task<RegistrationResult> RegisterUser(UserRegistrationDto userRegistrationDto);
        Task<bool> VerifyEmail(string email, string token);
        
    }
}
