// ApplicationUser.cs

using Microsoft.AspNetCore.Identity;

namespace Module.Identity.Domain.Models;

public class ApplicationUser : IdentityUser<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedOn { get; set; }
    
    // public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
}