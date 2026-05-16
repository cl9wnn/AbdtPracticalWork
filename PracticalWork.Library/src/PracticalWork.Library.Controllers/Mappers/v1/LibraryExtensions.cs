using PracticalWork.Library.Contracts.v1.Library.Statistics;
using PracticalWork.Library.Dtos;

namespace PracticalWork.Library.Controllers.Mappers.v1;

public static class LibraryExtensions
{
    public static GetLibraryStatisticsResponse ToLibraryStatisticsResponse(this LibraryStatisticsDto dto) =>
        new(
            NewBooksCount:dto.NewBooksCount,
            NewReadersCount: dto.NewReadersCount,
            BorrowedBooksCount: dto.BorrowedBooksCount,
            ReturnedBooksCount: dto.ReturnedBooksCount,
            OverdueBooksCount: dto.OverdueBooksCount
        );
}