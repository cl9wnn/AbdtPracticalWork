using PracticalWork.Library.SharedKernel.Exceptions;

namespace PracticalWork.Library.Exceptions;

/// <summary>
/// Исключение, возникающее при ошибках валидации загрузки изображений
/// </summary>
public class ValidateImageException: AppException
{
    public ValidateImageException(string message) : base($"{message}")
    {
    }

    public ValidateImageException(string message, Exception innerException) : base(message, innerException)
    {
    }
}