using PracticalWork.Library.SharedKernel.Exceptions;

namespace PracticalWork.Library.Exceptions;

/// <summary>
/// Исключение уровня сервиса книг
/// </summary>
public sealed class BookServiceException : AppException
{
    public BookServiceException(string message) : base($"{message}")
    {
    }

    public BookServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}