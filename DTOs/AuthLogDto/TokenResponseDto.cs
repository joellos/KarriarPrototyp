namespace CC_Karriarpartner.DTOs.AuthLogDto
{
    public class TokenResponseDto
    {
        // What you get when you login
        public required string AccessToken { get; set; } // JWT token
        public required string RefreshToken { get; set; } // Refresh token for getting new access token
    }
}