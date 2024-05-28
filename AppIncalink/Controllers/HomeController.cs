using AppIncalink.Datos;
using AppIncalink.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.IO;
using QuestPDF.Fluent;

namespace AppIncalink.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly actividadesDatos _actividadesDatos;


        public HomeController(ILogger<HomeController> logger, actividadesDatos actividadesDatos)
        {
            _logger = logger;
            _actividadesDatos = actividadesDatos;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GenerarPdf()
        {
            var actividades = _actividadesDatos.listar();
            var pdfStream = _actividadesDatos.GeneratePdf(actividades);

            return new FileStreamResult(pdfStream, "application/pdf")
            {
                FileDownloadName = "Actividades.pdf"
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
