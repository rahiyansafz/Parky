﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Parky.Web.Models;
using Parky.Web.Models.ViewModel;
using Parky.Web.Repository.IRepository;
using System.Diagnostics;
using System.Security.Claims;

namespace Parky.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INationalParkRepository _nationalParkRepository;
    private readonly ITrailRepository _trailRepository;
    private readonly IAccountRepository _accountRepository;

    public HomeController(ILogger<HomeController> logger, INationalParkRepository nationalParkRepository,
            ITrailRepository trailRepository, IAccountRepository accountRepository)
    {
        _logger = logger;
        _nationalParkRepository = nationalParkRepository;
        _trailRepository = trailRepository;
        _accountRepository = accountRepository;
    }

    public async Task<IActionResult> Index()
    {
        IndexVM listOfParksAndTrails = new()
        {
            NationalParkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")!),
            TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWToken")!),
        };
        return View(listOfParksAndTrails);
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

    [HttpGet]
    public IActionResult Login()
    {
        User obj = new();
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(User obj)
    {
        User objUser = await _accountRepository.LoginAsync(SD.AccountAPIPath + "authenticate/", obj);
        if (objUser.Token is null)
        {
            return View();
        }

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, objUser.Username));
        identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


        HttpContext.Session.SetString("JWToken", objUser.Token);
        TempData["alert"] = "Welcome " + objUser.Username;
        return RedirectToAction("Index");
    }
}
