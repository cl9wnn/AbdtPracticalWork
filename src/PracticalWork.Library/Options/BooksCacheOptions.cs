namespace PracticalWork.Library.Options;

/// <summary>
/// Настройки кэширования для функциональности книг
/// </summary>
public class BooksCacheOptions
{
    /// <summary> Общий префикс для инвалидации кэша</summary>
    public string BooksCacheVersionPrefix { get; set; }  
        
    /// <summary> Настройки кэширования для списков книг</summary>
    public CacheEntryOptions BooksListCache { get; set; } 
    
    /// <summary> Настройки кэширования для детальной информации о книге</summary>
    public CacheEntryOptions BookDetailsCache { get; set; }
    
    /// <summary> Настройки кэширования для книг библиотеки</summary>
    public CacheEntryOptions LibraryBooksCache { get; set; }
}