using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Domain.Models;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Common.Response;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.SearchFeature.Handler;

 public  record SearchDictionaryQuery :  IQuery<PagedResult<DictionaryEntryDto>>
    {
        public SearchDictionaryDto SearchDictionary { get; set; } = new();
        public long? UserId { get; set; }
    }
    
public class SearchDictionaryQueryHandler(DictionaryModuleDbContext dbContext)
    : IQueryHandler<SearchDictionaryQuery, PagedResult<DictionaryEntryDto>>
{
    public async Task<PagedResult<DictionaryEntryDto>> Handle(SearchDictionaryQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DictionaryEntry, bool>> searchPredicate = request.SearchDictionary.SearchType switch
        {
            SearchType.MustMatch => e => e.Gloss.ToLower() == request.SearchDictionary.SearchTerm.ToLower(),
            SearchType.Contains => e => e.Gloss.ToLower().Contains(request.SearchDictionary.SearchTerm.ToLower()),
            SearchType.StartsWith => e => e.Gloss.ToLower().StartsWith(request.SearchDictionary.SearchTerm.ToLower()),
            SearchType.EndsWith => e => e.Gloss.ToLower().EndsWith(request.SearchDictionary.SearchTerm.ToLower()),
            _ => e => e.Gloss.ToLower().Contains(request.SearchDictionary.SearchTerm.ToLower())
        };
        
        var totalCount = await dbContext.DictionaryEntries
            .AsNoTracking()
            .Where(searchPredicate)
            .CountAsync(cancellationToken);
            
        var entries = await dbContext.DictionaryEntries
            .AsNoTracking()
            .Where(searchPredicate)
            .OrderBy(e => e.Gloss)
            .Skip((request.SearchDictionary.PaginationParams.PageNumber - 1) * request.SearchDictionary.PaginationParams.PageSize)
            .Take(request.SearchDictionary.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);
            
        var userFavoriteIds = new List<long>();
        if (request.UserId.HasValue)
        {
            userFavoriteIds = await dbContext.UserFavoriteGlosses
                .Where(f => f.UserId == request.UserId.Value)
                .Select(f => f.DictionaryEntryId)
                .ToListAsync(cancellationToken);
        }
            
        var entryIds = entries.Select(e => e.Id).ToList();
        if (entryIds.Any())
        {
            if (request.UserId.HasValue)
            {
                await RecordSearchHistory(request.UserId.Value, request.SearchDictionary.SearchTerm, entries[0].Id, cancellationToken);
            }
            
            var entriesToUpdate = await dbContext.DictionaryEntries
                .Where(e => entryIds.Contains(e.Id))
                .ToListAsync(cancellationToken);
                
            foreach (var entry in entriesToUpdate)
            {
                entry.SearchCount++;
            }
            
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
        var result = entries.Select(e => new DictionaryEntryDto
        {
            Id = e.Id,
            Gloss = e.Gloss,
            VideoUrl = e.VideoUrl,
            Source = e.Source,
            SearchCount = e.SearchCount,
            IsFavorite = userFavoriteIds.Contains(e.Id),
            CreatedOn = e.CreatedOn,
            LastModifiedOn = e.LastModifiedOn
        }).ToList();
        
        return new PagedResult<DictionaryEntryDto>
        {
            Items = result,
            PageNumber = request.SearchDictionary.PaginationParams.PageNumber,
            PageSize = request.SearchDictionary.PaginationParams.PageSize,
            TotalCount = totalCount
        };
    }
    
    private async Task RecordSearchHistory(long userId, string searchTerm, long matchedGlossId, CancellationToken cancellationToken)
    {
        var searchHistory = new UserSearchHistory
        {
            UserId = userId,
            SearchText = searchTerm,
            SearchDate = DateTime.UtcNow,
            MatchedDictionaryEntryId = matchedGlossId
        };
        
        dbContext.UserSearchesHistory.Add(searchHistory);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}