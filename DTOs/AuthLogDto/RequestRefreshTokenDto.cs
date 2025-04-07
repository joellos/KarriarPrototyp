namespace CC_Karriarpartner.DTOs.AuthLogDto
{
    public class RequestRefreshTokenDto
    {
        // what you send to get a new access token
        public int UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
