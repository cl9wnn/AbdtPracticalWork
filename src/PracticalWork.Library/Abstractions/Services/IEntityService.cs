namespace PracticalWork.Library.Abstractions.Services;

public interface IEntityService<TDto>
{
    Task<TDto> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}