using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Domain.Models;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.Create.Handler;

public record CreateDictionaryEntryCommand : CreateDictionaryEntryDto, ICommand<Result<DictionaryEntryDto>>;
    
public class CreateDictionaryEntryCommandHandler : ICommandHandler<CreateDictionaryEntryCommand, Result<DictionaryEntryDto>>
{
    private readonly DictionaryModuleDbContext _dbContext;
        
    public CreateDictionaryEntryCommandHandler(DictionaryModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<Result<DictionaryEntryDto>> Handle(CreateDictionaryEntryCommand request, CancellationToken cancellationToken)
    {
        var existingEntry = await _dbContext.DictionaryEntries
            .FirstOrDefaultAsync(e => e.Gloss.ToLower() == request.Gloss.ToLower(), cancellationToken);
                
        if (existingEntry != null)
        {
            return ErrorCode.DictionaryEntryAlreadyExists;
        }
            
        var entry = new DictionaryEntry
        {
            Gloss = request.Gloss,
            VideoUrl = request.VideoUrl,
            CreatedOn = DateTime.UtcNow
        };
            
        await _dbContext.DictionaryEntries.AddAsync(entry, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
            
        var result = new DictionaryEntryDto
        {
            Id = entry.Id,
            Gloss = entry.Gloss,
            VideoUrl = entry.VideoUrl,
            SearchCount = entry.SearchCount,
            CreatedOn = entry.CreatedOn,
            LastModifiedOn = entry.LastModifiedOn
        };
            
        return Result.Success(result);
    }
}