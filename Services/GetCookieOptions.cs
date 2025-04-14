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
                SameSite = SameSiteMode.Lax, // Set to Lax to work on Swagger, Change to None/strict to work on React
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            };
        }
        public static CookieOptions RefreshTokenCookie()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Lax,  // Set to Lax to work on Swagger, Change to None/strict to work on React
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
        }
    }
}
