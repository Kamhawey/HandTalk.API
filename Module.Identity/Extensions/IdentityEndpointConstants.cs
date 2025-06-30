namespace Module.Identity.Extensions;

public static class IdentityEndpointConstants
{
    private const string ApiBase = "/api";
    private const string IdentityBase = $"{ApiBase}/Identity";
    private const string EmailBase = $"{ApiBase}/Email";
    private const string PasswordBase = $"{ApiBase}/Pssword";
    
    public static class Routes
    {
        public const string Register = $"{IdentityBase}/Register";
        public const string Login = $"{IdentityBase}/Login";
        public const string RefreshToken = $"{IdentityBase}/RefreshToken";
        public const string FacebookLogin = $"{IdentityBase}/external-login/Facebook";
        public const string GoogleLogin = $"{IdentityBase}/external-login/Google";
        // public const string ExternalLoginCallback = $"{IdentityBase}/external-login-callback";
        
        public const string SendEmailConfirmation = $"{EmailBase}/SendEmailConfirmation";
        public const string ConfirmEmail = $"{EmailBase}/ConfirmEmail";
        
        public const string CreatePasswordResetToken = $"{PasswordBase}/CreatePasswordResetToken";
        public const string ResetPassword = $"{PasswordBase}/Reset";
        public const string ChangePassword = $"{PasswordBase}/Change";
        
        // Profile 
        public const string CurrentUser = $"{IdentityBase}/CurrentUser";
        public const string EditProfile = $"{IdentityBase}/EditProfile";
        
    }
    
    public static class Tags
    {
        public const string Authentication = "Authentication";
        public const string Email = "Email";
        public const string Password = "Password";
        public const string Profile = "Profile";

    }
}