using PracticalWork.Library.Contracts.v1.Readers.Request;
using PracticalWork.Library.Models;

namespace PracticalWork.Library.Controllers.Mappers.v1;

public static class ReadersExtensions
{
    public static Reader ToReader(this CreateReaderRequest request) =>
        new()
        {
           FullName = request.FullName,
           PhoneNumber = request.PhoneNumber,
           ExpiryDate = request.ExpiryDate,
        };
}