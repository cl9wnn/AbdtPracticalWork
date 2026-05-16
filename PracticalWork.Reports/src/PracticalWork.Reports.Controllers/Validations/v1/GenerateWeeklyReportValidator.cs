using FluentValidation;
using PracticalWork.Shared.Contracts.Http.Reports.WeeklyReport;

namespace PracticalWork.Reports.Controllers.Validations.v1;

public class GenerateWeeklyReportValidator : AbstractValidator<GenerateWeeklyReportRequest>
{
    public GenerateWeeklyReportValidator()
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

        RuleFor(x => x.NewBooksCount)
            .NotNull()
            .WithMessage("Поле обязательно для заполнения!")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество новых книг не может быть отрицательным!");

        RuleFor(x => x.NewReadersCount)
            .NotNull()
            .WithMessage("Поле обязательно для заполнения!")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество новых читателей не может быть отрицательным!");

        RuleFor(x => x.BorrowedBooksCount)
            .NotNull()
            .WithMessage("Поле обязательно для заполнения!")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество выданных книг не может быть отрицательным!");

        RuleFor(x => x.ReturnedBooksCount)
            .NotNull()
            .WithMessage("Поле обязательно для заполнения!")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество возвращенных книг не может быть отрицательным!");

        RuleFor(x => x.OverdueBooksCount)
            .NotNull()
            .WithMessage("Поле обязательно для заполнения!")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество просроченных выдач не может быть отрицательным!");
    }
}