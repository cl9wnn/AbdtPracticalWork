using FluentValidation;
using PracticalWork.Library.Contracts.v1.Readers.Request;

namespace PracticalWork.Library.Controllers.Validations.v1.Readers;

public class ExtendReaderRequestValidator: AbstractValidator<ExtendReaderRequest>
{
    public ExtendReaderRequestValidator()
    {
        RuleFor(x => x.NewExpiryDate)
            .NotEmpty().WithMessage("Новая дата окончания срока действия должна быть указана.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Новая дата окончания срока действия должна быть в будущем.");;
    }
}