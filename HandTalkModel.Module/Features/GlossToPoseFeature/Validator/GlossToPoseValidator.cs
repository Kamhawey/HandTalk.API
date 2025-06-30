using FluentValidation;
using HandTalkModel.Module.Features.GlossToPoseFeature.Handler;

namespace HandTalkModel.Module.Features.GlossToPoseFeature.Validator;

public class GlossToPoseValidator : AbstractValidator<GlossToPoseQuery>
{
    public GlossToPoseValidator()
    {
        RuleFor(x => x.Gloss)
            .NotEmpty().WithMessage("Gloss is required")
            .MaximumLength(500).WithMessage("Gloss cannot exceed 500 characters");
    }
}