namespace Module.Identity.AuthFeatures.ExternalLoginFeature.Config;

public class GoogleAuthConfig
{
    public const string SectionName = "Authentication:Google";
    public string ClientId { get; set; }
    public string ClientSecret { get; set; } 
}