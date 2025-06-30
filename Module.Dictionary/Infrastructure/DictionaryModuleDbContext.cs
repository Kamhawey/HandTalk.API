using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Domain.Models;
using Shared.Consts;

namespace Module.Dictionary.Infrastructure;

public class DictionaryModuleDbContext : DbContext
{
    protected string Schema => Schemas.Dictionary;
        
    public DictionaryModuleDbContext(DbContextOptions<DictionaryModuleDbContext> options) : base(options)
    {
    }
        
    public DbSet<DictionaryEntry> DictionaryEntries { get; set; }
    public DbSet<UserFavoriteGloss> UserFavoriteGlosses { get; set; }
    public DbSet<UserSearchHistory> UserSearchesHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        if (!string.IsNullOrWhiteSpace(Schema))
        {
            modelBuilder.HasDefaultSchema(Schema);
        }
            
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            
        modelBuilder.Entity<DictionaryEntry>().ToTable("DictionaryEntries");
        modelBuilder.Entity<DictionaryEntry>().HasIndex(d => d.Gloss);
        modelBuilder.Entity<DictionaryEntry>().HasIndex(d => d.SearchCount);
        
        modelBuilder.Entity<UserFavoriteGloss>().ToTable("UserFavoriteGlosses");
        modelBuilder.Entity<UserFavoriteGloss>().HasIndex(f => new { f.UserId, f.DictionaryEntryId }).IsUnique();
            
        modelBuilder.Entity<UserFavoriteGloss>()
            .HasOne(uf => uf.DictionaryEntry)
            .WithMany(d => d.UserFavorites)
            .HasForeignKey(uf => uf.DictionaryEntryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserSearchHistory>()
            .HasOne(sh => sh.MatchedDictionaryEntry)
            .WithMany()
            .HasForeignKey(sh => sh.MatchedDictionaryEntryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserSearchHistory>()
            .HasIndex(x => new { x.UserId, x.SearchDate });    }
}