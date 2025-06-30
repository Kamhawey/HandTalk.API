using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Domain.Models;
using Shared.Consts;

namespace Module.Identity.Infrastructure;

public class IdentityModuleDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,long>
{
    protected string Schema => Schemas.Identity;
    
    public IdentityModuleDbContext(DbContextOptions<IdentityModuleDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        if (!string.IsNullOrWhiteSpace(Schema))
        {
            modelBuilder.HasDefaultSchema(Schema);
        }
        
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<long>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<long>>().ToTable("UserTokens");

        modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");
    }
    public DbSet<RefreshToken> RefreshTokens { get; set; }      
}