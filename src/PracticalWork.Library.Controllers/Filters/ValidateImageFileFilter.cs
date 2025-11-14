using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using PracticalWork.Library.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace PracticalWork.Library.Controllers.Filters;

public class ValidateImageFileFilter : IAsyncActionFilter
{
    private readonly Dictionary<string, string[]> _allowedMimeTypes = new()
    {
        [".jpg"] = ["image/jpeg"],
        [".jpeg"] = ["image/jpeg"],
        [".png"] = ["image/png"],
        [".webp"] = ["image/webp"]
    };

    private readonly long _maxFileSizeBytes;

    public ValidateImageFileFilter(long maxFileSizeBytes)
    {
        _maxFileSizeBytes = maxFileSizeBytes;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.HasFormContentType)
        {
            throw new ValidateImageException("Ожидался form-data запрос!");
        }

        var files = context.HttpContext.Request.Form.Files;

        if (files.Count == 0)
        {
            throw new ValidateImageException("Файл не был загружен!");
        }

        foreach (var file in files)
        {
            await ValidateFile(file);
        }

        await next();
    }

    private async Task ValidateFile(IFormFile file)
    {
        if (file.Length == 0)
        {
            throw new ValidateImageException("Файл не может быть пустым!");
        }

        if (file.Length > _maxFileSizeBytes)
        {
            throw new ValidateImageException("Файл превышает максимально разрешимый размер!");
        }

        await ValidateMimeTypeAsync(file);
    }

    private async Task ValidateMimeTypeAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var buffer = new byte[20];

        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

        if (bytesRead < 12)
        {
            throw new ValidateImageException("Файл слишком мал для определения типа");
        }

        var mimeType = GetMimeType(buffer);
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_allowedMimeTypes.TryGetValue(extension, out var expectedMimeTypes))
        {
            throw new ValidateImageException($"Неподдерживаемое расширение файла: {extension}");
        }

        if (!expectedMimeTypes.Contains(mimeType))
        {
            throw new ValidateImageException(
                $"Несоответствие MIME типа файла. Ожидался: {string.Join(", ", expectedMimeTypes)}, а определен как: {mimeType}");
        }
    }

    private string GetMimeType(byte[] fileBytes)
    {
        if (fileBytes.Length >= 12)
        {
            if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8 && fileBytes[2] == 0xFF)
                return "image/jpeg";

            if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47 &&
                fileBytes[4] == 0x0D && fileBytes[5] == 0x0A && fileBytes[6] == 0x1A && fileBytes[7] == 0x0A)
                return "image/png";

            if (fileBytes[0] == 'R' && fileBytes[1] == 'I' && fileBytes[2] == 'F' && fileBytes[3] == 'F' &&
                fileBytes[8] == 'W' && fileBytes[9] == 'E' && fileBytes[10] == 'B' && fileBytes[11] == 'P')
                return "image/webp";
        }

        return "unknown";
    }
}