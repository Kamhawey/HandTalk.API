using Microsoft.AspNetCore.Identity;

namespace Module.Identity.Domain.Models;

public class ApplicationRole : IdentityRole<long>
{
    public string? Description { get; set; }
}