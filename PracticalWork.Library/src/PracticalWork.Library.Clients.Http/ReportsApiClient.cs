using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Shared.Contracts.Http.Reports.WeeklyReport;

namespace PracticalWork.Library.Clients.Http;

/// <summary>
/// Клиент для взаимодействия с API сервиса отчетов
/// </summary>
public class ReportsApiClient : IReportsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReportsApiClient> _logger;
    
    public ReportsApiClient(HttpClient httpClient, ILogger<ReportsApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    /// <inheritdoc cref="IReportsApiClient.GenerateWeeklyReport"/>
    public async Task<GenerateWeeklyReportResponse> GenerateWeeklyReport(
        GenerateWeeklyReportRequest request,
        CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/reports/generate-weekly", request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);

            _logger.LogError(
                "Сервис отчетов вернул ошибку {StatusCode}: {Error}",
                response.StatusCode,
                error);

            throw new HttpRequestException($"Ошибка сервиса отчетов: {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<GenerateWeeklyReportResponse>(ct);

        return result ?? throw new InvalidOperationException("Сервис отчетов вернул пустой ответ (null)");
    }
}