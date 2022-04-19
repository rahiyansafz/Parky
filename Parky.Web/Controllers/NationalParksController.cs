﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parky.Web.Models;
using Parky.Web.Repository.IRepository;

namespace Parky.Web.Controllers;

[Authorize]
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

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Upsert(int? id)
    {
        NationalPark obj = new();

        if (id is null)
        {
            //this will be true for Insert/Create
            return View(obj);
        }

        //Flow will come here for update
        obj = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken")!);
        if (obj is null)
        {
            return NotFound();
        }
        return View(obj);
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

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var status = await _nationalParkRepository.DeleteAsync(SD.NationalParkAPIPath, id, HttpContext.Session.GetString("JWToken")!);
        if (status)
        {
            return Json(new { success = true, message = "Delete Successful" });
        }
        return Json(new { success = false, message = "Delete Not Successful" });
    }
}
