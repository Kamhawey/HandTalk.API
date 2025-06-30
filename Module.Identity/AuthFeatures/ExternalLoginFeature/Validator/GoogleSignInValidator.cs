using FluentValidation;
using Module.Identity.AuthFeatures.ExternalLoginFeature.Handler;

namespace Module.Identity.AuthFeatures.ExternalLoginFeature.Validator;

public class GoogleSignInValidator : AbstractValidator<GoogleSignInCommand>
{
    public GoogleSignInValidator()
    {
        RuleFor(x => x.IdToken)
            .NotEmpty().WithMessage("Google ID token is required");
    }
}