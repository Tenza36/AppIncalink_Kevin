using AppIncalink.Datos;
using AppIncalink.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.IO;
using QuestPDF.Fluent;
using AppIncalink.Permisos;

namespace AppIncalink.Controllers
{
    [ValidarSesion] // Atributo aplicado a nivel de clase
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly actividadesDatos _actividadesDatos;
        private readonly reportesDatos _reporteDatos;
        grupoDatos _grupoDatos = new grupoDatos();

        public HomeController(ILogger<HomeController> logger, actividadesDatos actividadesDatos, reportesDatos reportesDatos)
        {
            _logger = logger;
            _actividadesDatos = actividadesDatos;
            _reporteDatos = reportesDatos;
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

        public class ComprasViewModel
        {
            public int IdGrupo { get; set; }
            public List<comprasModel> Compras { get; set; }
        }

        // Controlador modificado
        public IActionResult ListarCompras(int idGrupo)
        {
            var compras = _grupoDatos.ListaCompras(idGrupo);
            var viewModel = new ComprasViewModel
            {
                IdGrupo = idGrupo,
                Compras = compras
            };
            return View(viewModel);
        }
        public async Task<IActionResult> GenerarComprasPdf(int idGrupo)
        {
            var compras = _reporteDatos.ListaCompras(idGrupo);
            var pdfStream = _reporteDatos.GenerateComprasPdf(compras);

            return new FileStreamResult(pdfStream, "application/pdf")
            {
                FileDownloadName = "Compras.pdf"
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
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("usuario");
            return RedirectToAction("Login", "Acceso");
        }

    }
}
