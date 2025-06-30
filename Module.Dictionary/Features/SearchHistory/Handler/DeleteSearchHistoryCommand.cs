using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Common;

namespace Module.Dictionary.Features.SearchHistory.Handler;


public record DeleteSearchHistoryCommand : ICommand<Result<bool>>
{
    public long SearchHistoryId { get; set; }
    public long? UserId { get; set; }
}

public class DeleteSearchHistoryCommandHandler(DictionaryModuleDbContext dbContext)
    : ICommandHandler<DeleteSearchHistoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteSearchHistoryCommand request, CancellationToken cancellationToken)
    {
        if (!request.UserId.HasValue)
        {
            return false;
        }

        var searchHistory = await dbContext.UserSearchesHistory
            .FirstOrDefaultAsync(sh => sh.Id == request.SearchHistoryId 
                                       && sh.UserId == request.UserId.Value, 
                cancellationToken);

        if (searchHistory == null)
        {
            return false;
        }

        dbContext.UserSearchesHistory.Remove(searchHistory);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}