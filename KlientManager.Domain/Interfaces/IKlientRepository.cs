using KlientManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlientManager.Domain.Interfaces
{
    public interface IKlientRepository
    {
        void DeleteKlient(int id);
        void DeleteKlients(IEnumerable<int> id);

        int AddKlient(Domain.Entity.Klienci klient);

        Klienci GetKlientById(int id);


        IQueryable<Klienci> GetAllKlients();


        void UpdateKlient(Klienci klient);
    }
}
