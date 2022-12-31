using Booking.Data;
using Booking.Data.Repository;
using Booking.Model;
using Booking.Web.helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Booking.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IUnitOfWork uow;

        public HotelController(ApplicationDbContext db, IUnitOfWork uow)
        {
            this.db = db;
            this.uow = uow;
        }

        // GET: Hotel
        public async Task<IActionResult> Index()
        {

            ViewData["City"] = new SelectList(db.Hotel, "City", "City");
            ViewData["Country"] = new SelectList(db.Hotel, "Country", "Country");

            return View(await db.Hotel.ToListAsync());
        }

        // GET: Hotel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Hotel == null)
            {
                return NotFound();
            }

            var hotel = await db.Hotel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotel/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id = 0)
        {
            if (id == 0)
            {
                Hotel model = new Hotel();
                return View(model);
            }
            else
            {
                var model = uow.HotelRepository.GetById(id);
                if (model == null)
                {
                    return NotFound();
                }
                return View(model);
            }
        }

        // POST: Hotel/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Rating,City,Country,Address")] Hotel model)
        {
            string msg = "Something went wrong!";
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0)
                    {
                        msg = "Saved Successfully";
                        model.UserName = User.FindFirstValue(ClaimTypes.Name);
                        uow.HotelRepository.Add(model);//add
                    }
                    else
                    {
                        msg = "Updated Successfully";
                        model.UserName = User.FindFirstValue(ClaimTypes.Name);
                        uow.HotelRepository.Update(model);//update
                    }
                    bool status = await uow.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsModelExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var models = uow.HotelRepository.GetAllAsync();
                return Json(new { isValid = true, message = msg, html = Helper.RenderRazorViewToString(this, "_ViewAll", models.Result.ToList()) });
            }
            return Json(new { isValid = false, message = "Something went wrong!", html = Helper.RenderRazorViewToString(this, "Edit", model) });

        }

        // POST: Hotel/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!uow.HotelRepository.IsModelExist(id))
            {
                return Problem("Not Found.");
            }
            var model = uow.HotelRepository.GetById(id);
            if (model != null)
            {
                uow.HotelRepository.Delete(id);
                var result = uow.SaveAsync();
            }
            return Json(new { isValid = true, message = "Deleted Successfully", html = Helper.RenderRazorViewToString(this, "_ViewAll", db.Hotel.ToList()) });
        }

        private bool IsModelExists(int id)
        {
            return uow.HotelRepository.IsModelExist(id);
        }
        public async Task<IActionResult> GetSearchList(string city, string country, int rf, int rt)
        {
            var models = await uow.HotelRepository.GetAllByParamAsync(city, country, rf, rt);
            return Json(new { isValid = true, message = "", html = Helper.RenderRazorViewToString(this, "_ViewAll", models) });
        }
    }
}
