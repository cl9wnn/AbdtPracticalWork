using PracticalWork.Library.SharedKernel.Exceptions;

namespace PracticalWork.Library.Exceptions;

/// <summary>
/// Исключение уровня сервиса карточек читателей
/// </summary>
public class ReaderServiceException: AppException
{
    public ReaderServiceException(string message): base($"{message}")
    {
    }

    public ReaderServiceException(string message, Exception innerException): base(message, innerException)
    {
    }
}