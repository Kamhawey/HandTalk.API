using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.GetPopularGlosses.Handler;

public record RandomGlossesQuery(int Count = 10) : IQuery<List<DictionaryEntryDto>>;
    
public class PopularGlossesQueryHandler : IQueryHandler<RandomGlossesQuery, List<DictionaryEntryDto>>
{
    private readonly DictionaryModuleDbContext _dbContext;
        
    public PopularGlossesQueryHandler(DictionaryModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<List<DictionaryEntryDto>> Handle(RandomGlossesQuery request, CancellationToken cancellationToken)
    {
        var count = Math.Min(Math.Max(1, request.Count), 100);
            
        var entries = await _dbContext.DictionaryEntries
            .AsNoTracking()
            .OrderByDescending(e => e.SearchCount)
            .Take(count)
            .ToListAsync(cancellationToken);
                
        return entries.Select(e => new DictionaryEntryDto
        {
            Id = e.Id,
            Gloss = e.Gloss,
            VideoUrl = e.VideoUrl,
            SearchCount = e.SearchCount,
            CreatedOn = e.CreatedOn,
            LastModifiedOn = e.LastModifiedOn,
            Source = e.Source,
        }).ToList();
    }
}