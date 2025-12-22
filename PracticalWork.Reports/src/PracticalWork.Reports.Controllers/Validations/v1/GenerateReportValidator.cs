using FluentValidation;
using PracticalWork.Reports.Contracts.v1.Reports.Generate;

namespace PracticalWork.Reports.Controllers.Validations.v1;

public class GenerateReportValidator : AbstractValidator<GenerateReportRequest>
{
    public GenerateReportValidator()
    {
        RuleFor(x => x.PeriodFrom)
            .NotEmpty()
            .WithMessage("Дата начала периода обязательна!");

        RuleFor(x => x.PeriodTo)
            .NotEmpty()
            .WithMessage("Дата окончания периода обязательна!");

        RuleFor(x => x)
            .Must(x => x.PeriodFrom <= x.PeriodTo)
            .WithMessage("Дата начала периода не может быть позднее даты конца периода!");

        RuleFor(x => x.EventType)
            .NotEmpty().WithMessage("Тип активности обязателен!")
            .IsInEnum().WithMessage("Неверный тип активности!");
    }
}