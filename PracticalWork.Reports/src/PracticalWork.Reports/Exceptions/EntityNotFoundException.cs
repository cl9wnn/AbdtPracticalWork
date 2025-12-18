using PracticalWork.Reports.SharedKernel.Exceptions;

namespace PracticalWork.Reports.Exceptions;

/// <summary>
/// Исключение, возникающее при попытке доступа к сущности, которая не была найдена
/// </summary>
public class EntityNotFoundException : AppException
{
    public EntityNotFoundException(string message) : base($"{message}")
    {
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}