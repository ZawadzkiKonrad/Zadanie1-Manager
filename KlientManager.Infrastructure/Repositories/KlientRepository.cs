using KlientManager.Domain.Entity;
using KlientManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlientManager.Infrastructure.Repositories
{
    public class KlientRepository : IKlientRepository
    {
        private readonly Context _context;
        public KlientRepository(Context context)
        {
            _context = context;
        }
        public int AddKlient(Klienci klient)
        {
            _context.Klients.Add(klient);
            _context.SaveChanges();

            return klient.Id;
        }

        public void DeleteKlient(int id)
        {
            var klient = _context.Klients.FirstOrDefault(p => p.Id == id);

            _context.Klients.Remove(klient);
            _context.SaveChanges();
        }

        public void DeleteKlients(IEnumerable<int> id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Klienci> GetAllKlients()
        {
            var klients = _context.Klients;
            return klients;
        }

        public Klienci GetKlientById(int id)
        {
            var klient = _context.Klients.FirstOrDefault(p => p.Id == id);
            return klient;
        }

        public void UpdateKlient(Klienci klient)
        {
            //var newklient = _context.Klients.FirstOrDefault(x => x.Id == klient.Id);

            _context.Attach(klient);
            _context.Entry(klient).Property("Name").IsModified = true;
            _context.Entry(klient).Property("Surname").IsModified = true;
            _context.Entry(klient).Property("BirthYear").IsModified = true;
            _context.Entry(klient).Property("PESEL").IsModified = true;
            _context.Entry(klient).Property("Sex").IsModified = true;
            _context.SaveChanges();
        }
    }
}
