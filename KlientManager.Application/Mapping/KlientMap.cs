using CsvHelper.Configuration;
using KlientManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlientManager.Application.Mapping
{
    public sealed class KlientMap : ClassMap<Klienci>
    {
        public KlientMap()
        {
            Map(m => m.Name).Index(1).Name("Name");
            Map(m => m.Surname).Index(2).Name("Surname");
            Map(m => m.PESEL).Index(3).Name("PESEL");
            Map(m => m.BirthYear).Index(4).Name("BirthYear");
            Map(m => m.Sex).Index(5).Name("SEX");
        }

    }
}
