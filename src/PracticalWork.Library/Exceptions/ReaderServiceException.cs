using JetBrains.Annotations;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.SharedKernel.Exceptions;

namespace PracticalWork.Library.Exceptions;

/// <summary>
/// Исключение уровня сервиса карточек читателей
/// </summary>
public class ReaderServiceException: AppException
{
    [CanBeNull] private readonly IReadOnlyList<BorrowedBookDto> _borrowedBooks;

    public ReaderServiceException(string message): base($"{message}")
    {
    }

    public ReaderServiceException(string message, Exception innerException): base(message, innerException)
    {
    }

    public ReaderServiceException(string message, IReadOnlyList<BorrowedBookDto> borrowedBooks) : base($"{message}")
    {
        _borrowedBooks = borrowedBooks;
    }
}