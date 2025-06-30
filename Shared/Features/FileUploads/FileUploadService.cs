using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;
using Shared.DTOs.Files;

namespace Shared.Features.FileUploads;

public class FileUploadService : IFileUploadService
{
    private readonly IConfiguration _configuration;
    private readonly string _uploadPath;
    private readonly string _baseUrl;
    private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private readonly string[] _allowedImageMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };

    public FileUploadService(IConfiguration configuration)
    {
        _configuration = configuration;
        _uploadPath = "wwwroot/uploads";
        _baseUrl = _configuration["FileUpload:BaseUrl"] ?? "https://localhost:7000";

        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<Result<FileUploadResponse>> UploadFileAsync(IFormFile file, string folder = "uploads")
    {
        if (file.Length == 0)
        {
            return ErrorCode.FileRequired;
        }

        if (!IsValidFileSize(file))
        {
            return ErrorCode.FileSizeExceedsLimit;
        }

        try
        {
            var folderPath = Path.Combine(_uploadPath, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = GenerateUniqueFileName(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);
            var relativeFilePath = Path.Combine(folder, fileName).Replace("\\", "/");
            var fileUrl = $"{_baseUrl.TrimEnd('/')}/uploads/{relativeFilePath}";

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var response = new FileUploadResponse(
                fileName,
                relativeFilePath,
                fileUrl,
                file.Length,
                file.ContentType
            );

            return Result<FileUploadResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return ErrorCode.FileUploadFailed;
        }
    }



    public async Task<Result> DeleteFileAsync(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_uploadPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorCode.FileDeleteFailed);
        }
    }

    public bool IsValidImageFile(IFormFile file)
    {
        if (file == null) return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var contentType = file.ContentType.ToLowerInvariant();

        return _allowedImageExtensions.Contains(extension) &&
               _allowedImageMimeTypes.Contains(contentType);
    }

    public bool IsValidFileSize(IFormFile file, long maxSizeInBytes = 5 * 1024 * 1024)
    {
        return file != null && file.Length > 0 && file.Length <= maxSizeInBytes;
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var fileName = Path.GetFileNameWithoutExtension(originalFileName);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var guid = Guid.NewGuid().ToString("N")[..8];

        return $"{fileName}_{timestamp}_{guid}{extension}";
    }
}