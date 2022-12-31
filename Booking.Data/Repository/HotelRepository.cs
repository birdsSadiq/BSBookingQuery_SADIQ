using Booking.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Data.Repository
{
    public interface IHotelRepository : IDisposable
    {
        Task<IEnumerable<Hotel>> GetAllByParamAsync(string city, string country, int rf, int rt);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Hotel GetById(int id);
        bool IsNameExist(string name, int id);
        bool IsModelExist(int id);
        void Add(Hotel model);
        void Update(Hotel model);
        void Delete(int id);
        //Task<bool> SaveAsync();
    }

    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext dc;
        public HotelRepository(ApplicationDbContext dc)
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

        public async Task<IEnumerable<Hotel>> GetAllByParamAsync(string city, string country, int rf, int rt)
        {
            return await dc.Hotel.Where(d => d.Id > 0
            && (!string.IsNullOrEmpty(city) ? d.City == city : true)
            && (!string.IsNullOrEmpty(country) ? d.Country == country : true)
            && ((rf >= 0 && rt >= 0) ? (d.Rating > rf && d.Rating < rt) : true)).ToListAsync();
        }
        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await dc.Hotel.ToListAsync();
        }

        public Hotel GetById(int id)
        {
            return dc.Hotel.Find(id);
        }
        public bool IsNameExist(string name, int id)
        {
            return dc.Hotel.Where(d => d.Name.ToLower().Trim() == name.ToLower().Trim() && (id > 0 ? d.Id != id : true)).Count() > 0 ? true : false;
        }

        public bool IsModelExist(int id)
        {
            return dc.Hotel.Where(d => d.Id == id).Count() > 0 ? true : false;
        }

        public void Add(Hotel model)
        {
            dc.Hotel.Add(model);
        }
        public void Update(Hotel model)
        {
            dc.Attach(model);
            dc.Entry(model).State = EntityState.Modified;
        }

        public async void Delete(int id)
        {
            var model = dc.Hotel.Find(id);
            dc.Hotel.Remove(model);
        }

    }
}
