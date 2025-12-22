using FluentValidation;
using PracticalWork.Reports.Contracts.v1.ActivityLogs.Get;

namespace PracticalWork.Reports.Controllers.Validations.v1;

public class GetActivityLogsValidator: AbstractValidator<GetActivityLogsRequest>
{
    public GetActivityLogsValidator()
    {
        RuleFor(x => x.EventDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.EventDate.HasValue)
            .WithMessage("Дата активности должна быть не позднее текущего времени!");
        
        RuleFor(x => x.EventType)
            .IsInEnum()
            .When(x => x.EventType.HasValue)
            .WithMessage("Неверный тип активности!");
        
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .When(x => x.Page != default)
            .WithMessage("Минимальный номер страницы - 1");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .When(x => x.PageSize != default)
            .WithMessage("Размер страницы должен быть от 1 до 100 элементов");
    }
}