using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PracticalWork.Library.Web.Configuration;

/// <summary>
/// Фильтр предназначен для трансформации доменных исключений в Bad Request
/// </summary>
/// <typeparam name="TAppException"> Доменное исключение </typeparam>
[UsedImplicitly]
public class DomainExceptionFilter<TAppException> : IAsyncActionFilter where TAppException : Exception
{
    protected readonly ILogger Logger;

    public DomainExceptionFilter(ILogger<DomainExceptionFilter<TAppException>> logger)
    {
        Logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        if (HasException(resultContext))
        {
            TryHandleException(resultContext, resultContext.Exception);
        }
    }

    private static bool HasException(ActionExecutedContext context) => context.Exception != null && !context.ExceptionHandled;

    protected virtual void TryHandleException(ActionExecutedContext context, Exception exception)
    {
        if (exception is not TAppException)
            return;

        var problemDetails = BuildProblemDetails(exception);

        context.Result = new BadRequestObjectResult(problemDetails);
        context.ExceptionHandled = true;

        Logger.LogError(exception, "Unhandled domain exception. Transformed to Bad request (400).");
    }

    protected static ValidationProblemDetails BuildProblemDetails(Exception exception)
    {
        var exceptionName = exception.GetType().Name;
        var errorMessages = GetExceptionMessages(exception).ToArray();
        
        var problemDetails = new ValidationProblemDetails
        {
            Title = "Произошла ошибка во время выполнения запроса.",
            Errors = { { exceptionName, errorMessages } }
        };

        return problemDetails;
    }
    
    private static IEnumerable<string> GetExceptionMessages(Exception exception)
    {
        var current = exception;
        while (current != null)
        {
            yield return current.Message;
            current = current.InnerException;
        }
    }
}
