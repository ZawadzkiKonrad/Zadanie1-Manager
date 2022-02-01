using AspNetCoreHero.ToastNotification.Abstractions;
using ClosedXML.Excel;
using KlientManager.Application.Interfaces;
using KlientManager.Application.ViewModels.Import;
using KlientManager.Application.ViewModels.Klient;
using KlientManager.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Web.WebPages.Html;

namespace KlientManager.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly IKlientService _klientService;
        private readonly INotyfService _notyf;
        private readonly Context _context;
        private readonly ILogger<TestController> _logger;
        public TestController(IKlientService klientService, Context context, ILogger<TestController> logger, INotyfService notyf)
        {
            _logger = logger;
            _klientService = klientService;
            _notyf = notyf;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Jestem w Test/Index");
            var klient = _klientService.GetAllKlients();
            return View(klient);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var list = _klientService.GetBirthYearList();
            return View(new KlientVm()
            {
                BirthYearList = list
            }); ;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KlientVm model)
        {
            
            if (ModelState.IsValid)
            {
                var id = _klientService.AddKlient(model);
                _notyf.Success("Dane zostały przesłane do bazy!");
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogWarning("zle dane przy dodawaniu klienta, blad validacji");
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            var klient = _klientService.GetKlientForEdit(id);
            if (klient != null)
            {
                return View(klient);
            }
            else
            {
                _notyf.Error("Brak klienta do edycji!");
                _logger.LogWarning("Proba edycji nie stworzonego rekordu");
                return RedirectToAction("Index");
            }



        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(KlientVm model)
        {
            if (model != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _klientService.UpdateKlient(model);
                        _notyf.Success("Dane zostały zaktualizowane!");
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {

                    _notyf.Error("Dane nie zostały zaktualizowane!");
                    _logger.LogWarning("Proba edycji, zle wpisanie danych, blad validacji", e);
                    return RedirectToAction("Index");
                }


                return View(model);
            }
            else
            {
                _notyf.Error("Brak pliku do edycji!");
                _logger.LogWarning("Proba edycji nie stworzonego rekordu");
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int id)

        {
            if (id!=null)
            {


                try
                {
                    _klientService.DeleteKlient(id);
                    _notyf.Success("Dane zostały usunięte!");

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    _notyf.Error("Błąd - nie ma takiego klienta");
                    _logger.LogWarning("Proba usuniecia nie stworzonego rekordu", e);
                    return RedirectToAction("Index");
                    throw;
                }
            }
            else
            {
                _notyf.Error("Nie wybrałeś klientów do usunięcia");
                _logger.LogWarning("Proba usuniecia nie stworzonego rekordu");
                return RedirectToAction("Index");
            }




        } 
        [HttpGet]
        public IActionResult Import()

        {
            var model = new ImportVm();
            return View(model);
        }
        [HttpPost]
        public IActionResult Import(IFormFile file,ImportVm model)

        {
            if (model.FileTypeId==1)
            {
                if (file != null)
                {


                    try
                    {
                        _klientService.Import(file);
                        _notyf.Success("Dane zostały przesłane do bazy!");

                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {


                        _notyf.Error("Nieprawidłowy format pliku");
                        _logger.LogWarning("Proba importu - nieprawidlowy format", e);

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    _notyf.Error("Brak pliku");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (file != null)
                {
                    try
                    {
                        _klientService.ImportExcel(file);
                         _notyf.Success("Dane zostały przesłane do bazy!");

                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        _notyf.Error("Nieprawidłowy format pliku");
                        _logger.LogWarning("Proba importuExcel - nieprawidlowy format", e);

                        return RedirectToAction("Index");

                    }
                }
                else
                {
                    _notyf.Error("Brak pliku");

                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public IActionResult Export()
        {
            var klient = _klientService.GetAllKlients().ToList();
            var builder = new StringBuilder();
            builder.AppendLine(@"Id;Name;Surname;PESEL;BirthYear;SEX");
            foreach (var kli in klient)
            {
                builder.AppendLine(String.Format(@"{0};{1};{2};{3};{4};{5}", kli.Id, kli.Name, kli.Surname,kli.PESEL, kli.BirthYear,kli.Sex));
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Employeeinfo.csv");

        }
        public IActionResult ExportExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var users = _klientService.GetAllKlients().ToList();
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Surname";
                worksheet.Cell(currentRow, 4).Value = "PESEL";
                worksheet.Cell(currentRow, 5).Value = "BirthYear";
                worksheet.Cell(currentRow, 6).Value = "Sex";
                foreach (var user in users)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = user.Id;
                    worksheet.Cell(currentRow, 2).Value = user.Name;
                    worksheet.Cell(currentRow, 3).Value = user.Surname;
                    worksheet.Cell(currentRow, 4).Value = user.PESEL;
                    worksheet.Cell(currentRow, 5).Value = user.BirthYear;
                    worksheet.Cell(currentRow, 6).Value = user.Sex;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }

        }
    }
}
