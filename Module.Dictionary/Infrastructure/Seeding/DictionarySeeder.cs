using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Module.Dictionary.Domain.Models;
using System.Text.Json;

namespace Module.Dictionary.Infrastructure.Seeding;

public static class DictionarySeeder
{
    public static async Task SeedDictionaryEntriesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DictionaryModuleDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

        logger.LogInformation("Checking if dictionary entries need to be seeded");

        await context.Database.MigrateAsync();

        if (await context.DictionaryEntries.AnyAsync())
        {
            logger.LogInformation("Dictionary entries already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding dictionary entries from JSON file");

        try
        {
            string jsonFilePath = Path.Combine(Path.GetFullPath("wwwroot"), "converted_videos.json");
            if (!File.Exists(jsonFilePath))
            {
                logger.LogWarning("Dictionary data file not found at: {FilePath}", jsonFilePath);
                return;
            }

            var jsonString = await File.ReadAllTextAsync(jsonFilePath);
            var entries = JsonSerializer.Deserialize<List<GlossEntry>>(jsonString);

            if (entries == null || !entries.Any())
            {
                logger.LogWarning("No dictionary entries found in the JSON file");
                return;
            }

            logger.LogInformation("Found {Count} dictionary entries to seed", entries.Count);

            var dictionaryEntries = entries.Select(e => new DictionaryEntry
            {
                Gloss = e.Gloss,
                VideoUrl = e.VideoUrl,
                Source = e.Source,
                CreatedOn = DateTime.UtcNow
            }).ToList();

            await context.DictionaryEntries.AddRangeAsync(dictionaryEntries);
            await context.SaveChangesAsync();

            logger.LogInformation("Successfully seeded {Count} dictionary entries", dictionaryEntries.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while seeding dictionary entries");
        }
    }
}

internal class GlossEntry
{
    public string Gloss { get; set; }
    public string Source { get; set; }
    public string VideoUrl { get; set; }

}
