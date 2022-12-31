using Booking.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Data.Repository
{
    public interface IHotelCommentRepository : IDisposable
    {
        Task<IEnumerable<HotelComment>> GetAllAsync(int hotelid);
        HotelComment GetById(int id);
        bool IsNameExist(string name, int id);
        bool IsModelExist(int id);
        void Add(HotelComment model);
        void Update(HotelComment model);
        void Delete(int id);
        //Task<bool> SaveAsync();
    }

    public class HotelCommentRepository : IHotelCommentRepository
    {
        private readonly ApplicationDbContext dc;
        public HotelCommentRepository(ApplicationDbContext dc)
        {
            this.dc = dc;
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dc.Dispose();
                }
            }
            this.disposed = true;
        }

        public async Task<IEnumerable<HotelComment>> GetAllAsync(int hotelid)
        {
            return await dc.HotelComment.Where(d => d.HotelId == hotelid).ToListAsync();
        }

        public HotelComment GetById(int id)
        {
            return dc.HotelComment.Find(id);
        }
        public bool IsNameExist(string name, int id)
        {
            return dc.HotelComment.Where(d => d.Name.ToLower().Trim() == name.ToLower().Trim() && (id > 0 ? d.Id != id : true)).Count() > 0 ? true : false;
        }

        public bool IsModelExist(int id)
        {
            return dc.HotelComment.Where(d => d.Id == id).Count() > 0 ? true : false;
        }

        public void Add(HotelComment model)
        {
            dc.HotelComment.Add(model);
        }
        public void Update(HotelComment model)
        {
            dc.Attach(model);
            dc.Entry(model).State = EntityState.Modified;
        }

        public async void Delete(int id)
        {
            var model = dc.HotelComment.Find(id);
            dc.HotelComment.Remove(model);
        }

    }
}
