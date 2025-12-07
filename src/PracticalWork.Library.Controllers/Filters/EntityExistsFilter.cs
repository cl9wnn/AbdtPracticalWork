using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PracticalWork.Library.Abstractions.Services;
using PracticalWork.Library.Exceptions;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Controllers.Filters;

public class EntityExistsFilter<TService, TDto>(TService service, string paramName) : IAsyncActionFilter
    where TService : class, IEntityService<TDto>
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.TryGetValue(paramName, out var idObj) && idObj is Guid id)
        {
            await service.Exists(id);
        }

        await next();
    }
}