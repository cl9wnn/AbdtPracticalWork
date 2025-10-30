namespace PracticalWork.Library.SharedKernel.Abstractions;

public interface IEntityService<TDto>
{
    Task<TDto> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}