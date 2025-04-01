using CC_Karriarpartner.DTOs.AuthLogDto;
using CC_Karriarpartner.Models;

namespace CC_Karriarpartner.Services.AuthServices
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(LoginDto request);
    }
}