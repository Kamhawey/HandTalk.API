using FluentValidation;
using Module.Identity.EmailFeatures.Handler;

namespace Module.Identity.EmailFeatures.Validator;

public class SendEmailConfirmationValidator : AbstractValidator<SendEmailConfirmationCommand>
{
    public SendEmailConfirmationValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
    }
}