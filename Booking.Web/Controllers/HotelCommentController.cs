using Booking.Data;
using Booking.Data.Repository;
using Booking.Model;
using Booking.Web.helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Booking.Web.Controllers
{
    public class HotelCommentController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IUnitOfWork uow;

        public HotelCommentController(ApplicationDbContext context, IUnitOfWork uow)
        {
            this.db = context;
            this.uow = uow;
        }

        // GET: HotelComment
        public async Task<IActionResult> Index(int id = 0, string user = "")
        {
            var applicationDbContext = db.HotelComment.Where(d => d.HotelId == id).Include(h => h.Hotel).OrderByDescending(d => d.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HotelComment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.HotelComment == null)
            {
                return NotFound();
            }

            var hotelComment = await db.HotelComment
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelComment == null)
            {
                return NotFound();
            }

            return View(hotelComment);
        }

        // GET: HotelComment/Edit/5
        public async Task<IActionResult> Edit(int id = 0)
        {
            HotelComment model = new HotelComment();
            model.HotelId = id;
            model.UserName= User.FindFirstValue(ClaimTypes.Name);
            model.EntryDateTime = DateTime.Now;
            model.Show = true;
            return View(model);
        }

        // POST: HotelComment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HotelId,Name,Show")] HotelComment model)
        {
            string msg = "Something went wrong!";
            try
            {
                msg = "Saved Successfully";
                model.EntryDateTime = DateTime.Now;
                model.UserName = User.FindFirstValue(ClaimTypes.Name);
                uow.HotelCommentRepository.Add(model);//add
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
            var models = uow.HotelCommentRepository.GetAllAsync(model.HotelId ?? 0);
            return Json(new { isValid = true, message = msg, html = Helper.RenderRazorViewToString(this, "_ViewAll", models.Result.ToList()) });
        }

        // GET: HotelComment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.HotelComment == null)
            {
                return NotFound();
            }

            var hotelComment = await db.HotelComment
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
            if (db.HotelComment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HotelComment'  is null.");
            }
            var hotelComment = await db.HotelComment.FindAsync(id);
            if (hotelComment != null)
            {
                db.HotelComment.Remove(hotelComment);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IsModelExists(int id)
        {
            return db.HotelComment.Any(e => e.Id == id);
        }
    }
}
