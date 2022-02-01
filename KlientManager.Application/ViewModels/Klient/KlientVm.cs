using AutoMapper;
using KlientManager.Application.Mapping;
using KlientManager.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KlientManager.Application.ViewModels.Klient
{
    public class KlientVm:IMapFrom<Klienci>
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Surname { get; set; }

        [MaxLength(11)]
        public string? PESEL { get; set; }
        public int? BirthYear { get; set; }
        public int Sex { get; set; }
        public string? SexName { get; set; }
        public int BirthYear_Id { get; set; }
        public List <SelectListItem>? BirthYearList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Klienci, KlientVm>().ReverseMap();

        }
        public KlientVm()
        {
            BirthYearList = new List<SelectListItem>();
        }

    }
}
