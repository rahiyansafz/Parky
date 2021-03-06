using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parky.API.Models;
using Parky.API.Models.Dtos;
using Parky.API.Repository.IRepository;
using Swashbuckle.AspNetCore.Annotations;

namespace Parky.API.Controllers;

[Route("api/v{version:apiVersion}/nationalparks")]
//[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class NationalParksController : ControllerBase
{
    private readonly INationalParkRepository _nationalParkRepository;
    private readonly IMapper _mapper;

    public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
    {
        _nationalParkRepository = nationalParkRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    public ActionResult GetNationalParks()
    {
        var objList = _nationalParkRepository.GetNationalParks();
        var objDto = new List<NationalParkDto>();
        foreach (var obj in objList)
        {
            objDto.Add(_mapper.Map<NationalParkDto>(obj));
        }
        return Ok(objDto);
    }

    [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
    [Authorize]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    public ActionResult GetNationalPark(int nationalParkId)
    {
        var obj = _nationalParkRepository.GetNationalPark(nationalParkId);
        if (obj is null)
        {
            return NotFound();
        }
        var objDto = _mapper.Map<NationalParkDto>(obj);
        //var objDto = new NationalParkDto()
        //{
        //    Created = obj.Created,
        //    Id = obj.Id,
        //    Name = obj.Name,
        //    State = obj.State,
        //};
        return Ok(objDto);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    public ActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
    {
        if (nationalParkDto is null)
        {
            return BadRequest(ModelState);
        }
        if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
        {
            ModelState.AddModelError("", "National Park Exists!");
            return StatusCode(404, ModelState);
        }
        var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
        if (!_nationalParkRepository.CreateNationalPark(nationalParkObj))
        {
            ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
            return StatusCode(500, ModelState);
        }
        return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
    }

    [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
    [ProducesResponseType(204)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
    {
        if (nationalParkDto is null || nationalParkId != nationalParkDto.Id)
        {
            return BadRequest(ModelState);
        }

        var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
        if (!_nationalParkRepository.UpdateNationalPark(nationalParkObj))
        {
            ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkObj.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteNationalPark(int nationalParkId)
    {
        if (!_nationalParkRepository.NationalParkExists(nationalParkId))
        {
            return NotFound();
        }

        var nationalParkObj = _nationalParkRepository.GetNationalPark(nationalParkId);
        if (!_nationalParkRepository.DeleteNationalPark(nationalParkObj))
        {
            ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

}
