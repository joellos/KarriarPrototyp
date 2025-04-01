namespace CC_Karriarpartner.DTOs.AuthLogDto
{
    public class RequestRefreshTokenDto
    {
        public int UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
