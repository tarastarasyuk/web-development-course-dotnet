using AutoMapper;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.DAL.Models;

namespace TheaterCashRegister.BLL.MappingProfiles;

public class PerformanceProfile : Profile
{
    public PerformanceProfile()
    {
        CreateMap<Performance, PerformanceDto>();
        CreateMap<PerformanceDto, Performance>();
    }
}