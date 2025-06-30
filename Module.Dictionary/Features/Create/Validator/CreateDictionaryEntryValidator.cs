using FluentValidation;
using Module.Dictionary.Features.Create.Handler;

namespace Module.Dictionary.Features.Create.Validator;

public class CreateDictionaryEntryValidator : AbstractValidator<CreateDictionaryEntryCommand>
{
    public CreateDictionaryEntryValidator()
    {
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