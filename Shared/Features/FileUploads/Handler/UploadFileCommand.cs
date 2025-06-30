using Microsoft.AspNetCore.Http;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Files;

namespace Shared.Features.FileUploads.Handler;

public record UploadFileCommand(IFormFile File, string? Folder = null) : ICommand<Result<FileUploadResponse>>;

public class UploadFileCommandHandler(
    IFileUploadService fileUploadService)
    : ICommandHandler<UploadFileCommand, Result<FileUploadResponse>>
{
    public async Task<Result<FileUploadResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        
        var folder = request.Folder ?? "general";
        return await fileUploadService.UploadFileAsync(request.File, folder);
    }
}
