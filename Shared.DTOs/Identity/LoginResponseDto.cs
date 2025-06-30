namespace Shared.DTOs.Common.Identity;

public class LoginResponseDto
{
    public long UserId { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTimeOffset TokenExpiry { get; set; }
    public DateTimeOffset  RefreshTokenExpiry { get; set; }
    // public IEnumerable<string> Roles { get; set; } = new List<string>();
}