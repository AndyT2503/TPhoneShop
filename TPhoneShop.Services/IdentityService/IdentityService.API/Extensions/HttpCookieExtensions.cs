namespace IdentityService.API.Extensions
{
    public static class HttpCookieExtensions
    {
        private const string REFRESH_TOKEN_COOKIE_KEY = "refreshToken";
        public static void SetRefreshTokenCookie(
            this HttpResponse response,
            string refreshToken,
            DateTimeOffset expriesTime
        )
        {
            response.Cookies.Append(
                REFRESH_TOKEN_COOKIE_KEY,
                refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = expriesTime
                });
        }

        public static string GetRefreshTokenCookie(this HttpRequest request)
        {
            return request.Cookies[REFRESH_TOKEN_COOKIE_KEY] ?? "";
        }
    }
}
