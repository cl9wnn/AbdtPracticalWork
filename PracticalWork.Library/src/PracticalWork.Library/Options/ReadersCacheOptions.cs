namespace PracticalWork.Library.Options;

/// <summary>
/// Настройки кэширования для функциональности карточек читателей
/// </summary>
public class ReadersCacheOptions
{
    /// <summary> Общий префикс для инвалидации кэша</summary>
    public string ReadersCacheVersionPrefix { get; set; }  
    
    /// <summary>Настройки кэширования для списка взятых читателем книг</summary>
    public CacheEntryOptions ReadersBooksCache { get; set; }
}