using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Module.Identity.Infrastructure.JWT;

public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string[] Audiences { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public int TokenExpMinutes { get; set; }
    public int RefreshTokenExpMinutes { get; set; }
    public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
}
