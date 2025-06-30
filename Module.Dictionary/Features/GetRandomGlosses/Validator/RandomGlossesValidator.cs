using FluentValidation;
using Module.Dictionary.Features.GetRandomGlosses.Handler;

namespace Module.Dictionary.Features.GetRandomGlosses.Validator;

public class RandomGlossesValidator : AbstractValidator<RandomGlossesQuery>
{
    public RandomGlossesValidator()
    {
        RuleFor(x => x.Count)
            .InclusiveBetween(1, 100).WithMessage("Count must be between 1 and 100");
    }
}