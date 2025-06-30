namespace Shared.DTOs.Files;

public record FileUploadResponse(
    string FileName,
    string FilePath,
    string FileUrl,
    long FileSize,
    string ContentType
);