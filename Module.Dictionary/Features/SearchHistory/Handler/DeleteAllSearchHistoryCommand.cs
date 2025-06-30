using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Common;

namespace Module.Dictionary.Features.SearchHistory.Handler;

public record DeleteAllSearchHistoryCommand : ICommand<Result<bool>>
{
    public long? UserId { get; set; }
}

public class DeleteAllSearchHistoryCommandHandler(DictionaryModuleDbContext dbContext)
    : ICommandHandler<DeleteAllSearchHistoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteAllSearchHistoryCommand request, CancellationToken cancellationToken)
    {
        if (!request.UserId.HasValue)
        {
            return false;
        }

        var userSearchHistory = await dbContext.UserSearchesHistory
            .Where(sh => sh.UserId == request.UserId.Value)
            .ToListAsync(cancellationToken);

        if (!userSearchHistory.Any())
        {
            return true;
        }

        dbContext.UserSearchesHistory.RemoveRange(userSearchHistory);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}