using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Data.Repository
{
    public interface IUnitOfWork
    {
        IHotelRepository HotelRepository { get; }
        IHotelCommentRepository HotelCommentRepository { get; }
        IDemoRepository DemoRepository { get; }
        Task<bool> SaveAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dc;
        public IHotelRepository HotelRepository => new HotelRepository(dc);
        public IHotelCommentRepository HotelCommentRepository => new HotelCommentRepository(dc);
        public IDemoRepository DemoRepository => new DemoRepository(dc);
        public UnitOfWork(ApplicationDbContext dc)
        {
            this.dc = dc;
        }

        public async Task<bool> SaveAsync()
        {
            return await dc.SaveChangesAsync() > 0;//returns number of records >0 means changes successfull saved
        }


    }
}
