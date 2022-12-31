using Booking.Data;
using Booking.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Booking.Web.Controllers
{
    public class HotelCommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HotelComment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HotelComment.Include(h => h.Hotel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HotelComment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HotelComment == null)
            {
                return NotFound();
            }

            var hotelComment = await _context.HotelComment
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelComment == null)
            {
                return NotFound();
            }

            return View(hotelComment);
        }

        // GET: HotelComment/Create
        public IActionResult Create(int id = 0)
        {
            HotelComment model = new HotelComment();
            model.Show = true;
            model.UserName = User.FindFirstValue(ClaimTypes.Name);
            model.HotelId=id;

            ViewData["HotelId"] = new SelectList(_context.Hotel, "Id", "Name");
            return View(model);
        }

        // POST: HotelComment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HotelId,Name,Show,UserName")] HotelComment model)
        {
            if (ModelState.IsValid)
            {
                model.UserName = User.FindFirstValue(ClaimTypes.Name);
                model.EntryDateTime = DateTime.Now;                
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotel, "Id", "Name", model.HotelId);
            return View(model);
        }

        // GET: HotelComment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HotelComment == null)
            {
                return NotFound();
            }

            var hotelComment = await _context.HotelComment.FindAsync(id);
            if (hotelComment == null)
            {
                return NotFound();
            }
            ViewData["HotelId"] = new SelectList(_context.Hotel, "Id", "Name", hotelComment.HotelId);
            return View(hotelComment);
        }

        // POST: HotelComment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HotelId,Name,Show")] HotelComment model)
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
                    if (!HotelCommentExists(model.Id))
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
            ViewData["HotelId"] = new SelectList(_context.Hotel, "Id", "Name", model.HotelId);
            return View(model);
        }

        // GET: HotelComment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HotelComment == null)
            {
                return NotFound();
            }

            var hotelComment = await _context.HotelComment
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelComment == null)
            {
                return NotFound();
            }

            return View(hotelComment);
        }

        // POST: HotelComment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HotelComment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HotelComment'  is null.");
            }
            var hotelComment = await _context.HotelComment.FindAsync(id);
            if (hotelComment != null)
            {
                _context.HotelComment.Remove(hotelComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelCommentExists(int id)
        {
            return _context.HotelComment.Any(e => e.Id == id);
        }
    }
}
