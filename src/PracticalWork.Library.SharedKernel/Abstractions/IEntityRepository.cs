namespace PracticalWork.Library.SharedKernel.Abstractions;

public interface IEntityRepository<TKey, TDto>
{
    Task<TDto> GetByIdAsync(TKey id);
    Task<Guid> AddAsync(TDto dto);
    Task<TDto> UpdateAsync(TDto dto);
    Task DeleteAsync(TKey id);
    Task<ICollection<TDto>> GetAllAsync();
    Task ExistsAsync(TKey id);
}