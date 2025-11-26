using PracticalWork.Library.Data.PostgreSql.Entities;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Data.PostgreSql.Mappers.v1;

public static class ReaderEntityExtensions
{
    public static Reader ToReader(this ReaderEntity entity)
    {
        return new Reader
        {
            FullName = entity.FullName,
            PhoneNumber = entity.PhoneNumber,
            ExpiryDate = entity.ExpiryDate,
            IsActive = entity.IsActive
        };
    }
}