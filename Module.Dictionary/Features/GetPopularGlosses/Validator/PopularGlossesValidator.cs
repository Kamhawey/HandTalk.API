using FluentValidation;
using Module.Dictionary.Features.GetPopularGlosses.Handler;

namespace Module.Dictionary.Features.GetPopularGlosses.Validator;

public class PopularGlossesValidator : AbstractValidator<PopularGlossesQuery>
{
    public PopularGlossesValidator()
    {
        RuleFor(x => x.Count)
            .InclusiveBetween(1, 100).WithMessage("Count must be between 1 and 100");
    }
}