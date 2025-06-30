namespace Module.Identity.AuthFeatures.ExternalLoginFeature.Config;

public class FacebookAuthConfig
{
    public const string SectionName = "Authentication:Facebook";
    public string TokenValidationUrl { get; set; }
    public string UserInfoUrl { get; set; }
    public string AppId { get; set; }
    public string AppSecret { get; set; } 
}