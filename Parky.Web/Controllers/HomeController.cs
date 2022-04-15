using Microsoft.AspNetCore.Mvc;
using Parky.Web.Models;
using Parky.Web.Repository.IRepository;
using System.Diagnostics;

namespace Parky.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INationalParkRepository _nationalParkRepository;
    private readonly ITrailRepository _trailRepository;

    public HomeController(ILogger<HomeController> logger, INationalParkRepository nationalParkRepository,
            ITrailRepository trailRepository) // IAccountRepository accountRepository
    {
        _logger = logger;
        _nationalParkRepository = nationalParkRepository;
        _trailRepository = trailRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
