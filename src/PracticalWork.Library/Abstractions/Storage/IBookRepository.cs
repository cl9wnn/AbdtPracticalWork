using PracticalWork.Library.Models;
using PracticalWork.Library.SharedKernel.Abstractions;

namespace PracticalWork.Library.Abstractions.Storage;

public interface IBookRepository: IEntityRepository<Guid, Book>
{
}