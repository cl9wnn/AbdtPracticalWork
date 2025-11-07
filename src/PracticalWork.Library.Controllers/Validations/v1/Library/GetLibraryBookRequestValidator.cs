using FluentValidation;
using PracticalWork.Library.Contracts.v1.Library.Request;

namespace PracticalWork.Library.Controllers.Validations.v1.Library;

public class GetLibraryBookRequestValidator: AbstractValidator<GetLibraryBooksRequest>
{
    public GetLibraryBookRequestValidator()
    {
        RuleFor(x => x.BookCategory)
            .IsInEnum()
            .When(x => x.BookCategory != default)
            .WithMessage("Указана недопустимая категория книги.");

        RuleFor(x => x.Author)
            .MaximumLength(500).WithMessage("Имя автора не должно превышать 500 символов.")
            .When(x => !string.IsNullOrWhiteSpace(x.Author));
        
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