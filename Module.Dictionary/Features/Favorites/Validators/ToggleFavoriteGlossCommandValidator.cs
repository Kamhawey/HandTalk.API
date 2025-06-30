using FluentValidation;
using Module.Dictionary.Features.Favorites.Handlers;

namespace Module.Dictionary.Features.Favorites.Validators;

public class ToggleFavoriteGlossCommandValidator : AbstractValidator<ToggleFavoriteGlossCommand>
{
    public ToggleFavoriteGlossCommandValidator()
    {
        RuleFor(x => x.DictionaryEntryId)
            .GreaterThan(0).WithMessage("Dictionary entry ID must be greater than 0");
    }
}