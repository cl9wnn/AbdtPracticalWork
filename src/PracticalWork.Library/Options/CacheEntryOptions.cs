namespace PracticalWork.Library.Options;

public class CacheEntryOptions
{
    public string KeyPrefix { get; set; } 
    public int TtlMinutes { get; set; }
}