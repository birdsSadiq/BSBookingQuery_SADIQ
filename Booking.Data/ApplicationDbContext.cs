using Booking.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region tables
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<HotelComment> HotelComment { get; set; }
        public DbSet<HotelCommentReply> HotelCommentReply { get; set; }
        public DbSet<Demo> Demo { get; set; }
        #endregion
    }
}