using FluentValidation;
using Module.Identity.PasswordFeatures.ChangePasswordFeature.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.PasswordFeatures.ChangePasswordFeature.Validator;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .Matches(RegexPatterns.PasswordPattern).WithMessage("Password must be at least 8 characters and contain uppercase, lowercase, number, and special character");
    }
}