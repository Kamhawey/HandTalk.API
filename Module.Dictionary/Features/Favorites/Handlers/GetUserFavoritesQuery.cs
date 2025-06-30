using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Dictionary.Favorites;

namespace Module.Dictionary.Features.Favorites.Handlers;

public  record GetUserFavoritesQuery(long UserId) : IQuery<List<UserFavoriteGlossDto>>;
    
public class GetUserFavoritesQueryHandler(DictionaryModuleDbContext dbContext)
    : IQueryHandler<GetUserFavoritesQuery, List<UserFavoriteGlossDto>>
{
    public async Task<List<UserFavoriteGlossDto>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
    {
        var favorites = await dbContext.UserFavoriteGlosses
            .Include(f => f.DictionaryEntry)
            .Where(f => f.UserId == request.UserId)
            .OrderByDescending(f => f.CreatedOn)
            .Select(f => new UserFavoriteGlossDto
            {
                Id = f.Id,
                DictionaryEntryId = f.DictionaryEntryId,
                Gloss = f.DictionaryEntry.Gloss,
                VideoUrl = f.DictionaryEntry.VideoUrl,
                FavoritedOn = f.CreatedOn
            })
            .ToListAsync(cancellationToken);
                
        return favorites;
    }
}