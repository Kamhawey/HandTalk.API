using FluentValidation;
using Module.Dictionary.Features.GetAll.Handler;

namespace Module.Dictionary.Features.GetAll.Validator;

public class GetAllDictionaryEntriesValidator : AbstractValidator<GetAllDictionaryEntriesQuery>
{
    public GetAllDictionaryEntriesValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");
                
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100");
    }
}