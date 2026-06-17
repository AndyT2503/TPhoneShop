namespace IdentityService.Domain.Constants
{
    public static class UserSecurityActions
    {
        public const string Login = "login";
        public const string Register = "register";
        public const string Logout = "logout";
        public const string ChangePassword = "change_password";
        public const string ResetPassword = "reset_password";
        public const string RefreshToken = "refresh_token";

        public static bool IsValid(string action)
        {
            return typeof(UserSecurityActions)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => (string)f.GetRawConstantValue()!)
                .Contains(action);
        }
    }
}
