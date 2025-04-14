namespace CC_Karriarpartner.Services
{
    public class GetCookieOptions
    {
        public static CookieOptions AccessTokenCookie()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Lax, // Set to Lax to allow cross-site requests, Change to None/strict later
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            };
        }
        public static CookieOptions RefreshTokenCookie()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Lax, // Set to Lax to allow cross-site requests, Change to None/strict later
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
        }
    }
}
