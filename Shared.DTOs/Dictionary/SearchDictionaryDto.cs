using Shared.DTOs.Common;

namespace Shared.DTOs.Dictionary;

public record  SearchDictionaryDto
{
    public string SearchTerm { get; set; }
    public SearchType SearchType { get; set; } = SearchType.MustMatch;
    public PaginationParams PaginationParams { get; set; } = new PaginationParams();
}