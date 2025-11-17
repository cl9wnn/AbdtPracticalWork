namespace PracticalWork.Library.Options;

public class BooksCacheOptions
{
    public CacheEntryOptions BooksListCache { get; set; } 
    public CacheEntryOptions BookDetailsCache { get; set; }
    public CacheEntryOptions LibraryBooksCache { get; set; }
}