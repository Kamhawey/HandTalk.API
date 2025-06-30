using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.GetRandomGlosses.Handler;

public record RandomGlossesQuery(int Count = 6) : IQuery<List<DictionaryEntryDto>>;

public class RandomGlossesQueryHandler(DictionaryModuleDbContext dbContext) : IQueryHandler<RandomGlossesQuery, List<DictionaryEntryDto>>
{
    public async Task<List<DictionaryEntryDto>> Handle(RandomGlossesQuery request, CancellationToken cancellationToken)
    {
        var entries = await dbContext.DictionaryEntries
            .AsNoTracking()
            .OrderBy(e => Guid.NewGuid())
            .Take(request.Count)
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