using FluentValidation;
using Module.Dictionary.Features.SearchFeature.Handler;

namespace Module.Dictionary.Features.SearchFeature.Validator;

public class SearchDictionaryValidator : AbstractValidator<SearchDictionaryQuery>
{
    public SearchDictionaryValidator()
    {
        RuleFor(x => x.SearchDictionary.SearchTerm)
            .NotEmpty().WithMessage("Search term is required");
                
        RuleFor(x => x.SearchDictionary.PaginationParams.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");
                
        RuleFor(x => x.SearchDictionary.PaginationParams.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100");
    }
}