namespace Shared.DTOs.Dictionary;

public record CreateDictionaryEntryDto
{
    public string Gloss { get; set; }
    public string VideoUrl { get; set; }
}
