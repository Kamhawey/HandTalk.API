namespace Shared.DTOs.Dictionary;

public record UpdateDictionaryEntryDto
{
    public long Id { get; set; }
    public string Gloss { get; set; }
    public string VideoUrl { get; set; }
}