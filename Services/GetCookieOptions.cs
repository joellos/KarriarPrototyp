namespace CC_Karriarpartner.Services
{
    public class GetCookieOptions
    {
        public static CookieOptions AccessTokenCookie()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            };
        }
        public static CookieOptions RefreshTokenCookie()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, 
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
        }
    }
}
