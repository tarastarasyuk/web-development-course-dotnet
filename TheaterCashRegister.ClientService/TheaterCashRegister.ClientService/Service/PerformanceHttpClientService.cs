using TheaterCashRegister.ClientService.DTO;
using TheaterCashRegister.ClientService.Exception;
using TheaterCashRegister.ClientService.Service.IService;
using TheaterCashRegister.ClientService.Util;

namespace TheaterCashRegister.ClientService.Service;

public class PerformanceHttpClientService : IPerformanceHttpClientService
{
    private const string BasePerformanceApiUrl = "/api/Performance";
    private const string RetrievePerformanceError = "An error occurred while retrieving performance.";
    private const string RetrievePerformancesError = "An error occurred while retrieving performances.";
    private const string CreatePerformanceError = "An error occurred while creating performance.";
    private const string DeletePerformanceError = "An error occurred while deleting performance.";

    private readonly HttpClient _httpClient;

    public PerformanceHttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IEnumerable<PerformanceDto> GetPerformancesAsync(string? title, string? author, string? genre,
        DateTime? date)
    {
        try
        {
            HttpResponseMessage response =
                _httpClient.GetAsync(
                        $"{BasePerformanceApiUrl}?title={title}&author={author}&genre={genre}&date={date?.ToString("s")}")
                    .GetAwaiter()
                    .GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);

            var performances = response.Content.ReadAsAsync<IEnumerable<PerformanceDto>>().GetAwaiter().GetResult();
            return performances;
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(RetrievePerformanceError, e);
        }
    }

    public PerformanceDto GetPerformanceAsync(int id)
    {
        try
        {
            HttpResponseMessage response =
                _httpClient.GetAsync($"{BasePerformanceApiUrl}/{id}").GetAwaiter().GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);

            var performance = response.Content.ReadAsAsync<PerformanceDto>().GetAwaiter().GetResult();
            return performance;
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(RetrievePerformancesError, e);
        }
    }

    public PerformanceDto CreatePerformanceAsync(CreatePerformanceRequestDto createPerformanceRequestDto)
    {
        try
        {
            HttpResponseMessage response =
                _httpClient.PostAsJsonAsync(BasePerformanceApiUrl, createPerformanceRequestDto).GetAwaiter()
                    .GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);

            var performance = response.Content.ReadAsAsync<PerformanceDto>().GetAwaiter().GetResult();
            return performance;
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(CreatePerformanceError, e);
        }
    }

    public void DeletePerformanceAsync(int id)
    {
        try
        {
            HttpResponseMessage response =
                _httpClient.DeleteAsync($"{BasePerformanceApiUrl}/{id}").GetAwaiter().GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(DeletePerformanceError, e);
        }
    }
}