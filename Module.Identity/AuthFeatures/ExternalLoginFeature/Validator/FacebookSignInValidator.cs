using FluentValidation;
using Module.Identity.AuthFeatures.ExternalLoginFeature.Handler;

namespace Module.Identity.AuthFeatures.ExternalLoginFeature.Validator;

public class FacebookSignInValidator : AbstractValidator<FacebookSignInCommand>
{
    public FacebookSignInValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Facebook access token is required");
    }
}