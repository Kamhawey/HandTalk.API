using Microsoft.EntityFrameworkCore;
using Module.Dictionary.Infrastructure;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.Update.Handler;

public record UpdateDictionaryEntryCommand : UpdateDictionaryEntryDto, ICommand<Result<DictionaryEntryDto>>;
    
    public class UpdateDictionaryEntryCommandHandler(DictionaryModuleDbContext dbContext)
        : ICommandHandler<UpdateDictionaryEntryCommand, Result<DictionaryEntryDto>>
    {
        public async Task<Result<DictionaryEntryDto>> Handle(UpdateDictionaryEntryCommand request, CancellationToken cancellationToken)
        {
            var entry = await dbContext.DictionaryEntries
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
                
            if (entry == null)
            {
                return ErrorCode.DictionaryEntryNotExists;
            }
            
            var existingEntry = await dbContext.DictionaryEntries
                .FirstOrDefaultAsync(e => e.Gloss.ToLower() == request.Gloss.ToLower() && e.Id != request.Id, cancellationToken);
                
            if (existingEntry != null)
            {
                return ErrorCode.DictionaryEntryAlreadyExists;
            }
            
            entry.Gloss = request.Gloss;
            entry.VideoUrl = request.VideoUrl;
            entry.LastModifiedOn = DateTime.UtcNow;
            
            await dbContext.SaveChangesAsync(cancellationToken);
            
            var result = new DictionaryEntryDto
            {
                Id = entry.Id,
                Gloss = entry.Gloss,
                VideoUrl = entry.VideoUrl,
                SearchCount = entry.SearchCount,
                Source = entry.Source,
                CreatedOn = entry.CreatedOn,
                LastModifiedOn = entry.LastModifiedOn
            };
            
            return Result.Success(result);
        }
    }