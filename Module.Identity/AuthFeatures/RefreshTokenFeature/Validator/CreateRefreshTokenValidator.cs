using FluentValidation;
using Module.Identity.AuthFeatures.RefreshTokenFeature.Handler;

namespace Module.Identity.AuthFeatures.RefreshTokenFeature.Validator;

public class CreateRefreshTokenValidator : AbstractValidator<CreateRefreshTokenCommand>
{
    public CreateRefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("RefreshToken is required");

        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("AccessToken is required");
    }
}