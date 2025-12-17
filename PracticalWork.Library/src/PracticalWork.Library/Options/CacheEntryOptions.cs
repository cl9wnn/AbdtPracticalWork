namespace PracticalWork.Library.Options;

/// <summary>
///  Универсальные настройки для записи в кэше
/// </summary>
public class CacheEntryOptions
{
    /// <summary> Префикс ключа для группировки записи кэша</summary>
    public string KeyPrefix { get; set; } 
    
    /// <summary> Время жизни ключа в кэше в минутах</summary>
    public int TtlMinutes { get; set; }
}