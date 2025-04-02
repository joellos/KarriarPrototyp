namespace CC_Karriarpartner.DTOs.AuthLogDto
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}