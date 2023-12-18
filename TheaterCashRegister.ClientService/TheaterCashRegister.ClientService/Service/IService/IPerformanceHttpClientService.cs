using TheaterCashRegister.ClientService.DTO;

namespace TheaterCashRegister.ClientService.Service.IService;

public interface IPerformanceHttpClientService
{
    IEnumerable<PerformanceDto> GetPerformancesAsync(string? author = null, string? title = null,
        string? genre = null, DateTime? date = null);

    PerformanceDto GetPerformanceAsync(int id);
    PerformanceDto CreatePerformanceAsync(CreatePerformanceRequestDto createPerformanceRequestDto);
    void DeletePerformanceAsync(int id);
}