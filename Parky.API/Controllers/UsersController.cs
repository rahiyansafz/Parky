using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky.API.Models;
using Parky.API.Repository.IRepository;

namespace Parky.API.Controllers;

[Authorize]
//[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/Users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] User model) // AuthenticationModel
    {
        var user = _userRepository.Authenticate(model.Username, model.Password);
        if (user is null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        return Ok(user);
    }
}
