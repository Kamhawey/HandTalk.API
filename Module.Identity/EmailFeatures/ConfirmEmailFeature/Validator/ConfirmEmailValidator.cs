using FluentValidation;
using Module.Identity.EmailFeatures.ConfirmEmailFeature.Handler;

namespace Module.Identity.EmailFeatures.ConfirmEmailFeature.Validator;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.VerificationCode)
            .NotEmpty().WithMessage("Verification code is required");
    }
}