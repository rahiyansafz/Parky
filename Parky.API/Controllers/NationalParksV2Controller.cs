using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky.API.Models.Dtos;
using Parky.API.Repository.IRepository;

namespace Parky.API.Controllers;

[Route("api/v{version:apiVersion}/nationalparks")]
[ApiVersion("2.0")]
[ApiController]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public class NationalParksV2Controller : ControllerBase
{
    private readonly INationalParkRepository _nationalParkRepository;
    private readonly IMapper _mapper;

    public NationalParksV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
    {
        _nationalParkRepository = nationalParkRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get list of national parks.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
    public IActionResult GetNationalParks()
    {
        var obj = _nationalParkRepository.GetNationalParks().FirstOrDefault();

        return Ok(_mapper.Map<NationalParkDto>(obj));
    }
}
