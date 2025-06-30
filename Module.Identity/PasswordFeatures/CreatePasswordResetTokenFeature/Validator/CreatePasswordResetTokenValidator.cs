using FluentValidation;
using Module.Identity.PasswordFeatures.CreatePasswordResetTokenFeature.Handler;

namespace Module.Identity.PasswordFeatures.CreatePasswordResetTokenFeature.Validator;


public class CreatePasswordResetTokenValidator : AbstractValidator<CreatePasswordResetTokenCommand>
{
    public CreatePasswordResetTokenValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
    }
}