using Booking.Model;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data.Repository
{
    public interface IDemoRepository : IDisposable
    {
        Task<IEnumerable<Demo>> GetAllAsync();
        Demo GetById(int id);
        bool IsNameExist(string name, int id);
        bool IsModelExist(int id);
        void Add(Demo model);
        void Update(Demo model);
        void Delete(int id);
        //Task<bool> SaveAsync();
    }

    public class DemoRepository : IDemoRepository
    {
        private readonly ApplicationDbContext dc;
        public DemoRepository(ApplicationDbContext dc)
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

        public async Task<IEnumerable<Demo>> GetAllAsync()
        {
            //string sql = string.Format(@"");
            //var a = dc.Database.ExecuteSqlRaw("");
            return await dc.Demo.ToListAsync();
        }

        public Demo GetById(int id)
        {
            return dc.Demo.Find(id);
        }
        public bool IsNameExist(string name, int id)
        {
            return dc.Demo.Where(d => d.Name.ToLower().Trim() == name.ToLower().Trim() && (id > 0 ? d.Id != id : true)).Count() > 0 ? true : false;
        }

        public bool IsModelExist(int id)
        {
            return dc.Demo.Where(d => d.Id == id).Count() > 0 ? true : false;
        }

        public void Add(Demo model)
        {
            dc.Demo.Add(model);
        }
        public void Update(Demo model)
        {
            dc.Attach(model);
            dc.Entry(model).State = EntityState.Modified;
        }

        public async void Delete(int id)
        {
            var model = dc.Demo.Find(id);
            dc.Demo.Remove(model);
        }

    }
}
