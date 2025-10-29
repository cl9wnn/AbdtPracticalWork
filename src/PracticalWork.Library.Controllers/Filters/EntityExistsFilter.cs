using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PracticalWork.Library.Abstractions.Services;

namespace PracticalWork.Library.Controllers.Filters;

public class EntityExistsFilter<TService, TDto>(TService service, string paramName): IAsyncActionFilter
    where TService : class, IEntityService<TDto>
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.TryGetValue(paramName, out var idObj) && idObj is Guid id)
        {
            var existsResult = await service.ExistsAsync(id);
            
            if (!existsResult)
            {
                context.Result = new NotFoundObjectResult(new { error = $"{typeof(TDto).Name} with id {id} not found." });
                return;
            }
        }
        
        await next();
    }
}