using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlientManager.Domain.Entity
{
    public class Klienci
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Surname { get; set; }

        [MaxLength(11)]
        public string PESEL { get; set; }
        public int BirthYear { get; set; }
        public int Sex { get; set; }
    }
}
