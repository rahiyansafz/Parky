using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parky.API.Models;
using Parky.API.Models.Dtos;
using Parky.API.Repository.IRepository;
using Swashbuckle.AspNetCore.Annotations;

namespace Parky.API.Controllers;

[Route("api/v{version:apiVersion}/trails")]
//[Route("api/Trails")]
[ApiVersion("1.0")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class TrailsController : ControllerBase
{
    private readonly ITrailRepository _trailRepository;
    private readonly IMapper _mapper;

    public TrailsController(ITrailRepository trailRepository, IMapper mapper)
    {
        _trailRepository = trailRepository;
        _mapper = mapper;
    }


    /// <summary>
    /// Get list of trails.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult GetTrails()
    {
        var objList = _trailRepository.GetTrails();
        var objDto = new List<TrailDto>();
        foreach (var obj in objList)
        {
            objDto.Add(_mapper.Map<TrailDto>(obj));
        }

        return Ok(objDto);
    }

    /// <summary>
    /// Get individual trail
    /// </summary>
    /// <param name="trailId"> The id of the trail </param>
    /// <returns></returns>
    [HttpGet("{trailId:int}", Name = "GetTrail")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TrailDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    //[Authorize(Roles = "Admin")]
    public IActionResult GetTrail(int trailId)
    {
        var obj = _trailRepository.GetTrail(trailId);
        if (obj is null)
        {
            return NotFound();
        }
        var objDto = _mapper.Map<TrailDto>(obj);

        return Ok(objDto);
    }

    [HttpGet("[action]/{nationalParkId:int}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TrailDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult GetTrailInNationalPark(int nationalParkId)
    {
        var objList = _trailRepository.GetTrailsInNationalPark(nationalParkId);
        if (objList is null)
        {
            return NotFound();
        }
        var objDto = new List<TrailDto>();
        foreach (var obj in objList)
        {
            objDto.Add(_mapper.Map<TrailDto>(obj));
        }

        return Ok(objDto);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(TrailDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
    {
        if (trailDto is null)
        {
            return BadRequest(ModelState);
        }
        if (_trailRepository.TrailExists(trailDto.Name))
        {
            ModelState.AddModelError("", "Trail Exists!");
            return StatusCode(404, ModelState);
        }
        var trailObj = _mapper.Map<Trail>(trailDto);
        if (!_trailRepository.CreateTrail(trailObj))
        {
            ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
            return StatusCode(500, ModelState);
        }

        return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
    }

    [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
    [ProducesResponseType(204)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
    {
        if (trailDto is null || trailId != trailDto.Id)
        {
            return BadRequest(ModelState);
        }

        var trailObj = _mapper.Map<Trail>(trailDto);
        if (!_trailRepository.UpdateTrail(trailObj))
        {
            ModelState.AddModelError("", $"Something went wrong when updating the record {trailObj.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteTrail(int trailId)
    {
        if (!_trailRepository.TrailExists(trailId))
        {
            return NotFound();
        }

        var trailObj = _trailRepository.GetTrail(trailId);
        if (!_trailRepository.DeleteTrail(trailObj))
        {
            ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
