using PracticalWork.Library.SharedKernel.Exceptions;

namespace PracticalWork.Library.Exceptions;

public class EntityNotFoundException : AppException
{
    public EntityNotFoundException(string message) : base($"{message}")
    {
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}