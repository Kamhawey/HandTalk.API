using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared.Features.FileUploads.Handler;

namespace Shared.Features.FileUploads.Validator;

public class UploadFileValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required")
            .Must(BeValidFileSize).WithMessage("File size must be less than 5MB")
            .Must(HaveFileName).WithMessage("File must have a valid filename");

        RuleFor(x => x.Folder)
            .Must(BeValidFolderName).WithMessage("Folder name contains invalid characters")
            .When(x => !string.IsNullOrEmpty(x.Folder));
    }

    private static bool BeValidFileSize(IFormFile? file)
    {
        if (file == null) return false;
        return file.Length > 0 && file.Length <= 5 * 1024 * 1024; // 5MB
    }

    private static bool HaveFileName(IFormFile? file)
    {
        return file != null && !string.IsNullOrEmpty(file.FileName);
    }

    private static bool BeValidFolderName(string? folder)
    {
        if (string.IsNullOrEmpty(folder)) return true;
        
        var invalidChars = Path.GetInvalidPathChars().Concat(['<', '>', ':', '"', '|', '?', '*']);
        return !folder.Any(invalidChars.Contains);
    }
}