using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Common.Response;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.GetAll.Handler;

public record GetAllDictionaryEntriesQuery : IQuery<PagedResult<DictionaryEntryDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public long? UserId { get; set; }
}

public class GetAllDictionaryEntriesQueryHandler(DictionaryModuleDbContext dbContext) 
    : IQueryHandler<GetAllDictionaryEntriesQuery, PagedResult<DictionaryEntryDto>>
{
    public async Task<PagedResult<DictionaryEntryDto>> Handle(GetAllDictionaryEntriesQuery request, CancellationToken cancellationToken)
    {
        if (request.PageNumber < 1)
            request.PageNumber = 1;
            
        if (request.PageSize < 1 || request.PageSize > 100)
            request.PageSize = 20;
            
        var totalCount = await dbContext.DictionaryEntries
            .AsNoTracking()
            .CountAsync(cancellationToken);
            
        var entries = await dbContext.DictionaryEntries
            .AsNoTracking()
            .OrderBy(e => e.Gloss)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
            
        var userFavoriteIds = new List<long>();
        if (request.UserId.HasValue)
        {
            userFavoriteIds = await dbContext.UserFavoriteGlosses
                .Where(f => f.UserId == request.UserId.Value)
                .Select(f => f.DictionaryEntryId)
                .ToListAsync(cancellationToken);
        }
        
        var result = entries.Select(e => new DictionaryEntryDto
        {
            Id = e.Id,
            Gloss = e.Gloss,
            VideoUrl = e.VideoUrl,
            SearchCount = e.SearchCount,
            IsFavorite = userFavoriteIds.Contains(e.Id),
            CreatedOn = e.CreatedOn,
            LastModifiedOn = e.LastModifiedOn,
            Source = e.Source
        }).ToList();
        
        return new PagedResult<DictionaryEntryDto>
        {
            Items = result,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}