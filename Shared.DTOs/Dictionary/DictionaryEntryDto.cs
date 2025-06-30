namespace Shared.DTOs.Dictionary;

public class DictionaryEntryDto
{
    public long Id { get; set; }
    public string Gloss { get; set; }
    
    public string Source { get; set; }
    public string VideoUrl { get; set; }
    public int SearchCount { get; set; }
    public bool IsFavorite { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}