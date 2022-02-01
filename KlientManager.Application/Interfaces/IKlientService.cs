using KlientManager.Application.ViewModels.Klient;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KlientManager.Application.Interfaces
{
    public interface IKlientService
    {
        IQueryable<KlientVm> GetAllKlients();
        int AddKlient(KlientVm klient);
        KlientVm GetKlientForEdit(int id);
        void UpdateKlient(KlientVm model);
        void DeleteKlient(int id);
        void DeleteKlients(IEnumerable<int> id);
        void Import(IFormFile file);
        void ImportExcel(IFormFile file);
        List<SelectListItem> GetTypeFileList();
        List<SelectListItem> GetBirthYearList();
       



    }
}
