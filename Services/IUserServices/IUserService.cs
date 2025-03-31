using CC_Karriarpartner.DTOs;

namespace CC_Karriarpartner.Services.IUserServices
{
    public interface IUserService
    {
        Task<bool> RegisterUser(UserRegistrationDto userDto);
        Task<bool> VerifyEmail(string email, string token);
    }
}
