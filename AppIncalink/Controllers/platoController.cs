using AppIncalink.Datos;
using AppIncalink.Models;
using AppIncalink.Permisos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace AppIncalink.Controllers
{
    [ValidarSesion]
    public class platoController : Controller
    {
        platosDatos _platosDatos = new platosDatos();
        public IActionResult Listar()
        {
            //La vista mostrara una lista
            var oLista = _platosDatos.listar();
            return View(oLista);
        }
        private List<SelectListItem> GetMenuOptions()
        {
            var menuDatos = new menuDatos();
            var menus = menuDatos.listar();
            var menuOptions = new List<SelectListItem>();
            foreach (var menu in menus)
            {
                menuOptions.Add(new SelectListItem
                {
                    Text = menu.nombre,
                    Value = menu.id.ToString()
                });
            }
            return menuOptions;
        }
        public string ObtenerNombrePorId(int? id, string tabla)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                string query = $"SELECT nombre FROM {tabla} WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@id", id.Value);

                var result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }

        public IActionResult Guardar(int id)
        {
            var recetas = _platosDatos.Obtener(id);

            if (recetas == null)
            {
                return NotFound(); // Retornar una vista de error 404 si la persona no se encuentra
            }

            var nombreMenu = ObtenerNombrePorId(recetas.idMenu, "menu");
           

            ViewBag.MenuOptions = new SelectList(GetMenuOptions(), "Value", "Text", recetas.idMenu);
      
            ViewBag.NombreMenu = nombreMenu;
           

            return View(recetas);
        }
        [HttpPost]
        public IActionResult Guardar(platosModel oPlatos)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MenuOptions = new SelectList(GetMenuOptions(), "Value", "Text", oPlatos.idMenu);
               
                return View(oPlatos);
            }

            var respuesta = _platosDatos.Guardar(oPlatos);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View(oPlatos);
        }

        public IActionResult ListarRecetas(int idMenu)
        {
            var recetas = _platosDatos.ListarRecetas(idMenu);
            return View(recetas);
        }
    }
}
