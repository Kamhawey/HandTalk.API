using FluentValidation;
using Module.Identity.AuthFeatures.LoginFeature.Handler;

namespace Module.Identity.AuthFeatures.LoginFeature.Validator;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}