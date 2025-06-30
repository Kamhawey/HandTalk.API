using FluentValidation;
using Module.Dictionary.Features.Update.Handler;

namespace Module.Dictionary.Features.Update.Validator;

public class UpdateDictionaryEntryValidator : AbstractValidator<UpdateDictionaryEntryCommand>
{
    public UpdateDictionaryEntryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");
                
        RuleFor(x => x.Gloss)
            .NotEmpty().WithMessage("Gloss is required")
            .MaximumLength(100).WithMessage("Gloss cannot exceed 100 characters");
                
        RuleFor(x => x.VideoUrl)
            .NotEmpty().WithMessage("Video URL is required")
            .MaximumLength(500).WithMessage("Video URL cannot exceed 500 characters")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Video URL must be a valid URL");
    }
}