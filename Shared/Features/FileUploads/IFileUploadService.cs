using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.DTOs.Files;

namespace Shared.Features.FileUploads;

public interface IFileUploadService
{
    Task<Result<FileUploadResponse>> UploadFileAsync(IFormFile file, string folder = "uploads");
    Task<Result> DeleteFileAsync(string filePath);
    bool IsValidImageFile(IFormFile file);
    bool IsValidFileSize(IFormFile file, long maxSizeInBytes = 5 * 1024 * 1024);
}

