using FluentValidation;
using Module.Dictionary.Features.Delete.Handler;

namespace Module.Dictionary.Features.Delete.Validator;

public class DeleteDictionaryEntryValidator : AbstractValidator<DeleteDictionaryEntryCommand>
{
    public DeleteDictionaryEntryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}