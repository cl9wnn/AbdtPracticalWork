using PracticalWork.Shared.Abstractions.Exceptions;

namespace PracticalWork.Library.Exceptions;

/// <summary>
/// Исключение уровня сервиса библиотеки
/// </summary>
public class LibraryServiceException: AppException
{
    public LibraryServiceException(string message): base($"{message}")
    {
    }

    public LibraryServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}