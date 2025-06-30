using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Dictionary;
using Shared.DTOs.Dictionary.SearchHistory;

namespace Module.Dictionary.Features.SearchHistory.Handler;

public record GetUserSearchHistoryQuery : IQuery<List<UserSearchHistoryDto>>
{
    public long? UserId { get; set; }
    public int Count { get; set; } = 10;
}


public class GetUserSearchHistoryQueryHandler(DictionaryModuleDbContext dbContext)
    : IQueryHandler<GetUserSearchHistoryQuery, List<UserSearchHistoryDto>>
{
    public async Task<List<UserSearchHistoryDto>> Handle(GetUserSearchHistoryQuery request, CancellationToken cancellationToken)
    {
        if (!request.UserId.HasValue)
        {
            return new List<UserSearchHistoryDto>();
        }
        
        var searchHistory = await dbContext.UserSearchesHistory
            .AsNoTracking()
            .Where(sh => sh.UserId == request.UserId.Value)
            .OrderByDescending(sh => sh.SearchDate)
            .Take(request.Count)
            .Include(sh => sh.MatchedDictionaryEntry)
            .Select(sh => new UserSearchHistoryDto
            {
                Id = sh.Id,
                SearchText = sh.SearchText,
                SearchDate = sh.SearchDate,
                MatchedResult = new DictionaryEntryDto
                {
                    Id = sh.MatchedDictionaryEntry.Id,
                    Gloss = sh.MatchedDictionaryEntry.Gloss,
                    VideoUrl = sh.MatchedDictionaryEntry.VideoUrl,
                    SearchCount = sh.MatchedDictionaryEntry.SearchCount,
                    Source = sh.MatchedDictionaryEntry.Source,
                    IsFavorite = dbContext.UserFavoriteGlosses
                        .Any(u => u.UserId == request.UserId 
                                  && u.DictionaryEntryId == sh.MatchedDictionaryEntry.Id),
                    CreatedOn = sh.MatchedDictionaryEntry.CreatedOn,
                    LastModifiedOn = sh.MatchedDictionaryEntry.LastModifiedOn
                }
            })
            .ToListAsync(cancellationToken);
            
        return searchHistory;
    }
}
