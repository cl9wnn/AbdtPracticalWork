using FluentValidation;
using PracticalWork.Library.Contracts.v1.Books.Request;

namespace PracticalWork.Library.Controllers.Validations.v1.Books;

public sealed class AddBookDetailsRequestValidator : AbstractValidator<AddBookDetailsRequest>
{
    public AddBookDetailsRequestValidator()
    {
        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание не может превышать 2000 символов.")
            .When(x => !string.IsNullOrEmpty(x.Description));
        
        When(x => x.CoverImage != null, () =>
        {
            RuleFor(x => x.CoverImage.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024)
                .WithMessage("Размер изображения не должен превышать 5 МБ.");

            RuleFor(x => x.CoverImage.FileName)
                .Must(f => new[] { ".jpg", ".jpeg", ".png", ".webp" }
                    .Contains(Path.GetExtension(f).ToLowerInvariant()))
                .WithMessage("Неверный формат изображения. Допустимые: jpg, jpeg, png, webp.");
        });
    }
}