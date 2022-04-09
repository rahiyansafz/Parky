using AutoMapper;
using Parky.API.Models;
using Parky.API.Models.Dtos;

namespace Parky.API.ParkyMapper;

public class ParkyMappings : Profile
{
    public ParkyMappings()
    {
        CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        CreateMap<Trail, TrailDto>().ReverseMap();
        CreateMap<Trail, TrailCreateDto>().ReverseMap();
        CreateMap<Trail, TrailUpdateDto>().ReverseMap();
    }
}
