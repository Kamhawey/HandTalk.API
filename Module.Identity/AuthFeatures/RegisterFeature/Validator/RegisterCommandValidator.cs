using FluentValidation;
using Module.Identity.Features.RegisterFeature.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.Features.RegisterFeature.Validator;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required")
            .MinimumLength(3).WithMessage("FirstName must be at least 3 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required")
            .MinimumLength(3).WithMessage("LastName must be at least 3 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .Matches(RegexPatterns.EmailPattern).WithMessage("Email is not valid")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .Matches(RegexPatterns.PasswordPattern).WithMessage("Password must be at least 8 characters and contain uppercase, lowercase, number, and special character");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}