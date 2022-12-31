using Booking.Data;
using Booking.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Booking.Web.Controllers
{
    public class HotelCommentReplyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelCommentReplyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HotelCommentReply
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HotelCommentReply.Include(h => h.HotelComment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HotelCommentReply/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HotelCommentReply == null)
            {
                return NotFound();
            }

            var hotelCommentReply = await _context.HotelCommentReply
                .Include(h => h.HotelComment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelCommentReply == null)
            {
                return NotFound();
            }

            return View(hotelCommentReply);
        }

        // GET: HotelCommentReply/Create
        public IActionResult Create()
        {
            ViewData["HotelCommentId"] = new SelectList(_context.Hotel, "Id", "Address");
            return View();
        }

        // POST: HotelCommentReply/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HotelId,HotelCommentId,Name,Show,UserName,EntryDateTime")] HotelCommentReply hotelCommentReply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotelCommentReply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelCommentId"] = new SelectList(_context.Hotel, "Id", "Address", hotelCommentReply.HotelCommentId);
            return View(hotelCommentReply);
        }

        // GET: HotelCommentReply/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HotelCommentReply == null)
            {
                return NotFound();
            }

            var hotelCommentReply = await _context.HotelCommentReply.FindAsync(id);
            if (hotelCommentReply == null)
            {
                return NotFound();
            }
            ViewData["HotelCommentId"] = new SelectList(_context.Hotel, "Id", "Address", hotelCommentReply.HotelCommentId);
            return View(hotelCommentReply);
        }

        // POST: HotelCommentReply/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HotelId,HotelCommentId,Name,Show,UserName,EntryDateTime")] HotelCommentReply hotelCommentReply)
        {
            if (id != hotelCommentReply.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelCommentReply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelCommentReplyExists(hotelCommentReply.Id))
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
            ViewData["HotelCommentId"] = new SelectList(_context.Hotel, "Id", "Address", hotelCommentReply.HotelCommentId);
            return View(hotelCommentReply);
        }

        // GET: HotelCommentReply/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HotelCommentReply == null)
            {
                return NotFound();
            }

            var hotelCommentReply = await _context.HotelCommentReply
                .Include(h => h.HotelComment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelCommentReply == null)
            {
                return NotFound();
            }

            return View(hotelCommentReply);
        }

        // POST: HotelCommentReply/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HotelCommentReply == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HotelCommentReply'  is null.");
            }
            var hotelCommentReply = await _context.HotelCommentReply.FindAsync(id);
            if (hotelCommentReply != null)
            {
                _context.HotelCommentReply.Remove(hotelCommentReply);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelCommentReplyExists(int id)
        {
            return _context.HotelCommentReply.Any(e => e.Id == id);
        }
    }
}
