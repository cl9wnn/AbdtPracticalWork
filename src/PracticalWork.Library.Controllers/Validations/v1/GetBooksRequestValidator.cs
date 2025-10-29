using FluentValidation;
using PracticalWork.Library.Contracts.v1.Books.Request;

namespace PracticalWork.Library.Controllers.Validations.v1;

public class GetBooksRequestValidator: AbstractValidator<GetBooksRequest>
{
    public GetBooksRequestValidator()
    {
        RuleFor(x => x.BookStatus)
            .IsInEnum()
            .When(x => x.BookStatus != default)
            .WithMessage("Указан недопустимый статус книги.");

        RuleFor(x => x.BookCategory)
            .IsInEnum()
            .When(x => x.BookCategory != default)
            .WithMessage("Указана недопустимая категория книги.");

        RuleFor(x => x.Author)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Author))
            .WithMessage("Имя автора превышает лимит.");

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