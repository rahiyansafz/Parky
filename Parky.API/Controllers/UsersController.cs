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
    public IActionResult Authenticate([FromBody] AuthenticationModel model)
    {
        var user = _userRepository.Authenticate(model.Username, model.Password);
        if (user is null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] AuthenticationModel model)
    {
        bool ifUserNameUnique = _userRepository.IsUniqueUser(model.Username);
        if (!ifUserNameUnique)
        {
            return BadRequest(new { message = "Username already exists" });
        }
        var user = _userRepository.Register(model.Username, model.Password);

        if (user is null)
        {
            return BadRequest(new { message = "Error while registering" });
        }

        return Ok();
    }
}
