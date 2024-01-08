using TheaterCashRegister.BLL.DTO;

namespace TheaterCashRegister.BLL.Service.IService;

public interface IPerformanceService
{
    PerformanceDto AddPerformance(PerformanceDto performanceDto);

    PerformanceDto GetPerformanceById(int id);

    bool DeletePerformance(int performanceId);

    IEnumerable<PerformanceDto> SearchPerformances(
        string? author = null, string? title = null, string? genre = null, DateTime? date = null);
}