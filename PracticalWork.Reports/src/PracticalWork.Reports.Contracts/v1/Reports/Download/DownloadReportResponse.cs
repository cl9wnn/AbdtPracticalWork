namespace PracticalWork.Reports.Contracts.v1.Reports.Download;

/// <summary>
/// Ответ на запрос о получении ссылки на скачивание отчета
/// </summary>
/// <param name="DownloadUrl"></param>
public sealed record DownloadReportResponse(string DownloadUrl);