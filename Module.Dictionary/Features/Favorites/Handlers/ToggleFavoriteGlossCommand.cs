using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Domain.Models;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;

namespace Module.Dictionary.Features.Favorites.Handlers;

 public  record ToggleFavoriteGlossCommand : ICommand<Result<bool>>
    {
        public long DictionaryEntryId { get; set; }
        public long UserId { get; set; }
    }
    
    public class ToggleFavoriteGlossCommandHandler(DictionaryModuleDbContext dbContext)
        : ICommandHandler<ToggleFavoriteGlossCommand, Result<bool>>
    {
        private const int MaxFavorites = 20;

        public async Task<Result<bool>> Handle(ToggleFavoriteGlossCommand request, CancellationToken cancellationToken)
        {
            var entryExists = await dbContext.DictionaryEntries
                .AnyAsync(d => d.Id == request.DictionaryEntryId, cancellationToken);
                
            if (!entryExists)
            {
                return ErrorCode.DictionaryEntryNotExists;
            }
            
            var existingFavorite = await dbContext.UserFavoriteGlosses
                .FirstOrDefaultAsync(f => 
                    f.UserId == request.UserId && 
                    f.DictionaryEntryId == request.DictionaryEntryId, 
                    cancellationToken);
                
            if (existingFavorite != null)
            {
                dbContext.UserFavoriteGlosses.Remove(existingFavorite);
                await dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success(false); 
            }
            else
            {
                var favoriteCount = await dbContext.UserFavoriteGlosses
                    .CountAsync(f => f.UserId == request.UserId, cancellationToken);
                    
                if (favoriteCount >= MaxFavorites)
                {
                    return ErrorCode.MaximumFavoritesReached;
                }
                
                var newFavorite = new UserFavoriteGloss
                {
                    UserId = request.UserId,
                    DictionaryEntryId = request.DictionaryEntryId,
                    CreatedOn = DateTime.UtcNow
                };
                
                await dbContext.UserFavoriteGlosses.AddAsync(newFavorite, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success(true);
            }
        }
    }