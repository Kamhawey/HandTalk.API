using FluentValidation;
using Module.Identity.PasswordFeatures.ResetPassword.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.PasswordFeatures.ResetPassword.Validator;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .Matches(RegexPatterns.PasswordPattern).WithMessage("Password must be at least 8 characters and contain uppercase, lowercase, number, and special character");
    }
}