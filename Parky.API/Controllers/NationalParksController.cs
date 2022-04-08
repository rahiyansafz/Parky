using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky.API.Models;
using Parky.API.Models.Dtos;
using Parky.API.Repository.IRepository;

namespace Parky.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NationalParksController : ControllerBase
{
    private readonly INationalParkRepository _nationalParkRepository;
    private readonly IMapper _mapper;

    public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
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

    /// <summary>
    /// Get individual national park
    /// </summary>
    /// <param name="nationalParkId"> The Id of the national Park </param>
    /// <returns></returns>
    [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
    [ProducesResponseType(200, Type = typeof(NationalParkDto))]
    [ProducesResponseType(404)]
    //[Authorize]
    [ProducesDefaultResponseType]
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
    [ProducesResponseType(201, Type = typeof(NationalParkDto))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
