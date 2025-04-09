using CC_Karriarpartner.DTOs;
using CC_Karriarpartner.DTOs.UserDtos;
using CC_Karriarpartner.Services.UserServices;

namespace CC_Karriarpartner.Services.IUserServices
{
    public interface IUserService
    {
        Task<RegistrationResult> RegisterUser(UserRegistrationDto userRegistrationDto);
        Task<bool> VerifyEmail(string email, string token);
        Task<List<UserPurchaseHistoryDto>> GetPurchaseHistory(int userId);
        Task<UserProfileDto> GetUserProfile(int userId);
        Task<RegistrationResult> UpdateUserProfile(int userId, UpdateProfileDto profileDto);
        Task <bool> DeleteUserProfile(int userId, string password);
    }
}
