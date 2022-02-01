using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using KlientManager.Application.Interfaces;
using KlientManager.Application.Mapping;
using KlientManager.Application.ViewModels.Klient;
using KlientManager.Domain.Entity;
using KlientManager.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KlientManager.Application.Services
{
    public class KlientService : IKlientService
    {
        private readonly IMapper _mapper;
        private readonly IKlientRepository _klientRepo;
        private readonly INotyfService _notyf;

        public KlientService(IKlientRepository klientRepo,
         IMapper mapper, INotyfService notyf)
        {
            _klientRepo = klientRepo;
            _mapper = mapper;
           _notyf = notyf;
        }
      
        public int AddKlient(KlientVm model)
        {
            var klient = _mapper.Map<Domain.Entity.Klienci>(model);
            if (model.SexName=="Kobieta")
            {
                klient.Sex = 1;
            }
            else
            {
                klient.Sex = 2;
            }
            var id = _klientRepo.AddKlient(klient);
            return id;
        }

        public void DeleteKlient(int id)
        {
            _klientRepo.DeleteKlient(id);
        }

        public void DeleteKlients(IEnumerable<int> id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<KlientVm> GetAllKlients()
        {
            var klients = _klientRepo.GetAllKlients()
            .ProjectTo<KlientVm>(_mapper.ConfigurationProvider);
            return klients;
        }

        public List<SelectListItem> GetBirthYearList()
        {
            var list = new List<SelectListItem>();
            for (int i = 1900; i < 2000; i++)
            {
                list.Add(new SelectListItem()
                {
                    Value=(i-1900).ToString(),
                    Text = i.ToString()
                }) ;
            }
            return list;
        }

        public KlientVm GetKlientForEdit(int id)
        {
            var klient = _klientRepo.GetKlientById(id);
            var klientVm = _mapper.Map<KlientVm>(klient);
            return klientVm;
        }

        public List<SelectListItem> GetTypeFileList()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "1",
                Text = "XLSX"
            });
            list.Add(new SelectListItem()
            {
                Value = "2",
                Text = "CSV"
            });

            return list;

        }

        public void Import(IFormFile file)
        {
            var read = file.OpenReadStream(); //IoFormFile to stream

            string name = file.FileName;

            using (var reader = new StreamReader(read))
            {

                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                {
                    csv.Context.RegisterClassMap<KlientMap>();

                    var records = csv.GetRecords<Klienci>().ToList();
                    foreach (var item in records)
                    {
                        _klientRepo.AddKlient(item);
                    }

                }

            }
        }

        public void ImportExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var list = new List<Klienci>();
            using (var stream = new MemoryStream())
            {
                file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    if (worksheet.Cells[1, 2].Value.ToString().Trim() == "Name" && worksheet.Cells[1, 3].Value.ToString().Trim() == "Surname")
                    {


                        for (int row = 2; row <= rowcount; row++)
                        {
                            list.Add(new Klienci
                            {

                                Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                Surname = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                PESEL = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                BirthYear = int.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
                                Sex = int.Parse(worksheet.Cells[row, 6].Value.ToString().Trim())
                            });

                        }
                        _notyf.Success("Dane zostały przesłane do bazy!");

                    }
                    else
                    {
                        _notyf.Error("Nieprawidłowy format pliku");
                    }
                }
            }
            foreach (var item in list)
            {
                _klientRepo.AddKlient(item);
            }
        }

        public void UpdateKlient(KlientVm model)
        {
            var klient = _mapper.Map<Domain.Entity.Klienci>(model);
            if (model.SexName == "Kobieta")
            {
                klient.Sex = 1;
            }
            else
            {
                klient.Sex = 2;
            }
            _klientRepo.UpdateKlient(klient);
        }
    }
}
