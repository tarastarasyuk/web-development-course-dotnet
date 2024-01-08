using System.Linq.Expressions;
using AutoMapper;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.BLL.Exception;
using TheaterCashRegister.BLL.Service.IService;
using TheaterCashRegister.DAL.Models;
using TheaterCashRegister.DAL.Repository.IRepository;

namespace TheaterCashRegister.BLL.Service;

public class PerformanceService : IPerformanceService
{
    private const string ErrorMessagePerformanceNotFound =
        "Error! Performance for given id was not found: performance id = {0}.";

    private const string ErrorMessagePerformanceExists =
        "Error! Performance specified title already exists: performance title = {0}.";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PerformanceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public PerformanceDto AddPerformance(PerformanceDto performanceDto)
    {
        var performanceDtoTitle = performanceDto.Title;
        if (ExistsByTitle(performanceDtoTitle))
        {
            throw new EntityDuplicateException(string.Format(ErrorMessagePerformanceExists, performanceDtoTitle));
        }

        var performance = _mapper.Map<Performance>(performanceDto);
        _unitOfWork.Performance.Add(performance);
        _unitOfWork.Save();
        return _mapper.Map<PerformanceDto>(performance);
    }

    private bool ExistsByTitle(string performanceDtoTitle)
    {
        Expression<Func<Performance, bool>> filter = p => p.Title.Contains(performanceDtoTitle);
        return _unitOfWork.Performance.GetAll(filter).Any();
    }

    public PerformanceDto GetPerformanceById(int id)
    {
        var performanceInternal = GetPerformanceInternal(id);
        return _mapper.Map<PerformanceDto>(performanceInternal);
    }

    public bool DeletePerformance(int performanceId)
    {
        var performance = GetPerformanceInternal(performanceId);
        _unitOfWork.Performance.Remove(performance);
        _unitOfWork.Save();
        return true;
    }

    public IEnumerable<PerformanceDto> SearchPerformances(
        string? author = null, string? title = null, string? genre = null, DateTime? date = null)
    {
        Expression<Func<Performance, bool>> filter = p =>
            (author == null || p.Author.Contains(author)) &&
            (title == null || p.Title.Contains(title)) &&
            (genre == null || p.Genre.Contains(genre)) &&
            (!date.HasValue || p.Date.Date == date.Value.Date);

        var performances = _unitOfWork.Performance.GetAll(filter);

        return performances.Select(p => _mapper.Map<PerformanceDto>(p)).ToList();
    }

    private Performance GetPerformanceInternal(int performanceId)
    {
        var performance = _unitOfWork.Performance.Get(p => p.Id == performanceId);

        if (performance == null)
        {
            throw new EntityNotFoundException(string.Format(ErrorMessagePerformanceNotFound, performanceId));
        }

        return performance;
    }
}