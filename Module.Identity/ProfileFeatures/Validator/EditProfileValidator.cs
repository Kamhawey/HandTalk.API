using FluentValidation;
using Module.Identity.ProfileFeatures.Handler;

namespace Module.Identity.ProfileFeatures.Validator;

public class EditProfileValidator : AbstractValidator<EditProfileCommand>
{
    public EditProfileValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");
    }


}