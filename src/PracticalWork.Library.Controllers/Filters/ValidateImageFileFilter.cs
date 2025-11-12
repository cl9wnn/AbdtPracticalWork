using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PracticalWork.Library.Controllers.Filters;

public class ValidateImageFileFilter: IAsyncActionFilter
{
    private readonly string[] _allowedExtensions = ["image/jpeg", "image/png", "image/webp"];
    private readonly long _maxFileSizeBytes = 5 * 1024 * 1024;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue("file", out var fileObj) || fileObj is not IFormFile file
                                                                          || file.Length == 0)
        {
            context.Result = new BadRequestObjectResult("Требуется файл!");
            return;
        }
        
        if (file.Length > _maxFileSizeBytes)
        {
            context.Result = new BadRequestObjectResult("Файл превышает максимально разрешимый размер!");
            return;
        }

        if (!_allowedExtensions.Contains(file.ContentType))
        {
            context.Result = new BadRequestObjectResult($"Неподдерживаемый формат файла: {file.ContentType}");
            return;
        }
        
        await next();
    }
}