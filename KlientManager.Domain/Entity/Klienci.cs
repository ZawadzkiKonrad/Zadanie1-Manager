using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlientManager.Domain.Entity
{
    public class Klienci
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string PESEL { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
        public int Sex { get; set; }
    }
}
