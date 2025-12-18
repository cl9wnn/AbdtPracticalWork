namespace PracticalWork.Reports.Abstractions.Services.Infrastructure;

/// <summary>
/// Контракт сервиса хранения файлов
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Сохраняет файл в хранилище.
    /// </summary>
    /// <param name="path">Путь или ключ, по которому будет сохранён файл</param>
    /// <param name="stream">Поток с содержимым файла</param>
    /// <param name="contentType">MIME-тип файла</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Полный URL или путь к сохранённому файлу</returns>
    Task<string> UploadFileAsync(string path, Stream stream, string contentType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Возвращает полный URL или путь к файлу из хранилища.
    /// </summary>
    /// <param name="path">Путь или ключ файла в хранилище</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Полный URL или путь к файлу</returns>
    Task<string> GetFilePathAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет файл из хранилища.
    /// </summary>
    /// <param name="path">Путь или ключ удаляемого файла</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task DeleteFileAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет наличие файла в хранилище.
    /// </summary>
    /// <param name="path">Путь или ключ файла</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns><c>true</c>, если файл существует; иначе — <c>false</c></returns>
    Task<bool> ExistsFileAsync(string path, CancellationToken cancellationToken = default);
}