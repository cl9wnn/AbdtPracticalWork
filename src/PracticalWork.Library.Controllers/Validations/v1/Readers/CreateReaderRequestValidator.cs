using FluentValidation;
using PracticalWork.Library.Contracts.v1.Readers.Request;

namespace PracticalWork.Library.Controllers.Validations.v1.Readers;

public class CreateReaderRequestValidator: AbstractValidator<CreateReaderRequest>
{
    public CreateReaderRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("ФИО читателя обязательно.")
            .Length(5, 200).WithMessage("ФИО должно содержать от 5 до 200 символов.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона не должен быть пустым.")
            .Matches(@"^(\+7|8)\d{10}$").WithMessage("Неверный формат номера телефона.");
        
        RuleFor(x => x.ExpiryDate)
            .NotEmpty().WithMessage("Дата окончания срока действия карточки обязательна")
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Дата окончания срока действия должна быть в будущем.");
    }
}