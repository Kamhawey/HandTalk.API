namespace Shared.DTOs.Dictionary.SearchHistory;

public class UserSearchHistoryDto
{
    public long Id { get; set; }
    public string SearchText { get; set; } = string.Empty;
    public  DictionaryEntryDto? MatchedResult  { get; set; }
    public DateTime SearchDate { get; set; }
}