namespace PracticalWork.Library.Data.Minio;

/// <summary>
///  Настройки для подключения к MinIO
/// </summary>
public class MinioOptions
{
    /// <summary> Конечная точка MinIO сервера</summary>
    public string Endpoint { get; set; }
    
    /// <summary> Ключ доступа (идентификатор пользователя)</summary>
    public string AccessKey { get; set; }
    
    /// <summary> Секретный ключ (пароль пользователя)</summary>
    public string SecretKey { get; set; }
    
    /// <summary>Название бакета для хранения файлов</summary>
    public string BucketName { get; set; }
    
    /// <summary> Использование SSL/TLS для подключения</summary>
    public bool UseSsl { get; set; }
}