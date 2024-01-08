using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheaterCashRegister.API.PL.DTO;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.BLL.Service.IService;

namespace TheaterCashRegister.API.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerTag("Manage performances.")]
public class PerformanceController : ControllerBase
{
    private readonly IPerformanceService _performanceService;
    private readonly IMapper _mapper;

    public PerformanceController(IPerformanceService performanceService, IMapper mapper)
    {
        _performanceService = performanceService;
        _mapper = mapper;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Retrieves all performances.")]
    public ActionResult<IEnumerable<PerformanceDto>> GetPerformances(string? title, string? author, string? genre,
        DateTime? date)
    { 
        var performances =
            _performanceService.SearchPerformances(author: author, title: title, genre: genre, date: date);
        return Ok(performances);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new performance.")]
    public ActionResult<PerformanceDto> CreatePerformance(CreatePerformanceRequestDto createPerformanceRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var performanceDto = _mapper.Map<PerformanceDto>(createPerformanceRequestDto);
        var createdPerformanceDto = _performanceService.AddPerformance(performanceDto);
        return CreatedAtAction(nameof(GetPerformance), new { id = createdPerformanceDto.Id }, createdPerformanceDto);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieves a specific performance by Id.")]
    public ActionResult<PerformanceDto> GetPerformance(int id)
    {
        var performanceDto = _performanceService.GetPerformanceById(id);
        return Ok(performanceDto);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a specific performance by Id.")]
    public IActionResult DeletePerformance(int id)
    {
        _performanceService.DeletePerformance(id);
        return NoContent();
    }
}