﻿using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(NationalPark obj)
    {
        if (ModelState.IsValid)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                byte[] p1 = null!;
                using (var fs1 = files[0].OpenReadStream())
                {
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                }
                obj.Picture = p1;
            }
            else
            {
                var objFromDb = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath, obj.Id, HttpContext.Session.GetString("JWToken")!);
                obj.Picture = objFromDb.Picture;
            }
            if (obj.Id == 0)
            {
                await _nationalParkRepository.CreateAsync(SD.NationalParkAPIPath, obj, HttpContext.Session.GetString("JWToken")!);
            }
            else
            {
                await _nationalParkRepository.UpdateAsync(SD.NationalParkAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken")!);
            }
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(obj);
        }
    }

    public async Task<IActionResult> GetAllNationalPark()
    {
        return Json(new { data = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")!) });
    }
}