using PracticalWork.Library.SharedKernel.Exceptions;

namespace PracticalWork.Library.Exceptions;

public class ValidateImageException: AppException
{
    public ValidateImageException(string message) : base($"{message}")
    {
    }

    public ValidateImageException(string message, Exception innerException) : base(message, innerException)
    {
    }
}