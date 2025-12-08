using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PracticalWork.Library.Exceptions;

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

    private static bool HasException(ActionExecutedContext context) =>
        context.Exception != null && !context.ExceptionHandled;

    protected virtual void TryHandleException(ActionExecutedContext context, Exception exception)
    {
        if (exception is TAppException)
        {
            var statusCode = GetStatusCode(exception);
            var problemDetails = BuildProblemDetails(exception, statusCode);

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;

            LogException(exception, statusCode);
            return;
        }

        var internalError = new ProblemDetails
        {
            Title = "Internal server error!",
            Status = StatusCodes.Status500InternalServerError
        };

        context.Result = new ObjectResult(internalError)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;

        Logger.LogError(exception, "Unexpected exception transformed to HTTP 500");
    }

    protected virtual int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status400BadRequest
        };
    }

    protected static ValidationProblemDetails BuildProblemDetails(Exception exception, int statusCode)
    {
        var exceptionName = exception.GetType().Name;
        var errorMessages = new[] { exception.Message };

        var problemDetails = new ValidationProblemDetails
        {
            Title = "Произошла ошибка во время выполнения запроса.",
            Status = statusCode,
            Errors = { { exceptionName, errorMessages } }
        };

        return problemDetails;
    }

    protected virtual void LogException(Exception exception, int statusCode)
    {
        Logger.LogError(exception,
            "Domain exception transformed to HTTP {StatusCode}. Message: {Message}",
            statusCode, exception.Message);
    }
}