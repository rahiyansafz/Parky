using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Parky.Web.Models;
using Parky.Web.Models.ViewModel;
using Parky.Web.Repository.IRepository;

namespace Parky.Web.Controllers
{
    [Authorize]
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;

        public TrailsController(INationalParkRepository nationalParkRepository, ITrailRepository trailRepository)
        {
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
        }

        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")!);

            TrailsVM objVM = new()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id is null)
            {
                //this will be true for Insert/Create
                return View(objVM);
            }

            //Flow will come here for update
            objVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken")!);
            if (objVM.Trail is null)
            {
                return NotFound();
            }
            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {

                if (obj.Trail.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, obj.Trail, HttpContext.Session.GetString("JWToken")!);
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath + obj.Trail.Id, obj.Trail, HttpContext.Session.GetString("JWToken")!);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")!);

                TrailsVM objVM = new()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWToken")!) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id, HttpContext.Session.GetString("JWToken")!);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}