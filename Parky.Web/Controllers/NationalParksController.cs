using Microsoft.AspNetCore.Mvc;
using Parky.Web.Models;
using Parky.Web.Repository.IRepository;

namespace Parky.Web.Controllers;

public class NationalParksController : Controller
{
    private readonly INationalParkRepository _nationalParkRepository;

    public NationalParksController(INationalParkRepository nationalParkRepository)
    {
        _nationalParkRepository = nationalParkRepository;
    }

    public IActionResult Index()
    {
        return View(new NationalPark() { });
    }

    public async Task<IActionResult> GetAllNationalPark()
    {
        return Json(new { data = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")!) });
    }
}
