using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;

namespace Module.Dictionary.Features.Delete.Handler;

    
public record DeleteDictionaryEntryCommand(long Id) : ICommand<Result>;
    
public class DeleteDictionaryEntryCommandHandler : ICommandHandler<DeleteDictionaryEntryCommand, Result>
{
    private readonly DictionaryModuleDbContext _dbContext;
        
    public DeleteDictionaryEntryCommandHandler(DictionaryModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<Result> Handle(DeleteDictionaryEntryCommand request, CancellationToken cancellationToken)
    {
        var entry = await _dbContext.DictionaryEntries
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
                
        if (entry == null)
        {
            return  ErrorCode.DictionaryEntryNotExists ;
        }
            
        _dbContext.DictionaryEntries.Remove(entry);
        await _dbContext.SaveChangesAsync(cancellationToken);
            
        return Result.Success();
    }
}