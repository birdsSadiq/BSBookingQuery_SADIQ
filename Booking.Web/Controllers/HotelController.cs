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
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;

        public HotelController(ApplicationDbContext context, IUnitOfWork uow)
        {
            _context = context;
            this.uow = uow;
        }

        // GET: Hotel
        public async Task<IActionResult> Index()
        {

            ViewData["City"] = new SelectList(_context.Hotel, "City", "City");
            ViewData["Country"] = new SelectList(_context.Hotel, "Country", "Country");

            return View(await _context.Hotel.ToListAsync());
        }

        // GET: Hotel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hotel == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotel/Create
        [Authorize]
        public IActionResult Create()
        {
            Hotel model = new Hotel();
            model.UserName = User.FindFirstValue(ClaimTypes.Name);
            return View();
        }

        // POST: Hotel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Rating,City,Country,Address")] Hotel model)
        {
            if (ModelState.IsValid)
            {
                model.UserName = User.FindFirstValue(ClaimTypes.Name);
                model.EntryDateTime = DateTime.Now;
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Hotel/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hotel == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Rating,City,Country,Address")] Hotel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.UserName = User.FindFirstValue(ClaimTypes.Name);
                    model.EntryDateTime = DateTime.Now;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Hotel/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hotel == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotel/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hotel'  is null.");
            }
            var hotel = await _context.Hotel.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotel.Remove(hotel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return _context.Hotel.Any(e => e.Id == id);
        }
        public async Task<IActionResult> GetSearchList(string city, string country, int rf, int rt)
        {
            var models = await uow.HotelRepository.GetAllByParamAsync(city, country, rf, rt);
            return Json(new { isValid = true, message = "", html = Helper.RenderRazorViewToString(this, "_ViewAll", models) });
        }
    }
}
