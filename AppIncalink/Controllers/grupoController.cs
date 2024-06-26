﻿using AppIncalink.Datos;
using AppIncalink.Models;
using AppIncalink.Permisos;
using Microsoft.AspNetCore.Mvc;

namespace AppIncalink.Controllers
{
    [ValidarSesion]
    public class grupoController : Controller
    {
        grupoDatos _grupoDatos = new grupoDatos();
        personaDatos _personaDatos = new personaDatos();


        public IActionResult Listar()
        {
            //La vista mostrara una lista
            var oLista = _grupoDatos.listar();
            return View(oLista);
        }
        public IActionResult Guardar()
        {
            //metodo que devuleve la vista
            return View();
        }

        //
        [HttpPost]
        public IActionResult Guardar(grupoModel oGrupo)
        {
            //Metodo recibe el obejto para guardarlo en bd
            if (!ModelState.IsValid)
                return View();
            var respuesta = _grupoDatos.Guardar(oGrupo);
            if (respuesta)

                return RedirectToAction("Listar");
            else return View();
        }

        public IActionResult Editar(int id)
        {
            //metodo que devuleve la vista
            var ohabitacion = _grupoDatos.Obtener(id);
            return View(ohabitacion);
        }
        [HttpPost]
        public IActionResult Editar(grupoModel oGrupo)
        {
            if (!ModelState.IsValid)
                return View();
            var respuesta = _grupoDatos.Editar(oGrupo);
            if (respuesta)

                return RedirectToAction("Listar");
            else return View();
        }
        public grupoController(grupoDatos grupoDatos)
        {
            _grupoDatos = grupoDatos;
        }
        public IActionResult DetallesPersona(int idGrupo, int idPersona)
        {
            var persona = _personaDatos.Obtener(idPersona);
            if (persona == null)
            {
                // Si no se encuentra la persona, puedes redirigir a una página de error o manejarlo de otra manera
                return RedirectToAction("Error");
            }

            return View(persona);
        }
        public IActionResult ListarPersonas(int idGrupo)
        {
            var personas = _grupoDatos.ObtenerPersonasPorGrupo(idGrupo);
            return View(personas);
        }
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

        public IActionResult Eliminar(int id)
        {
            //metodo que devuleve la vista
            var ogrupo = _grupoDatos.Obtener(id);
            return View(ogrupo);
        }
        [HttpPost]
        public IActionResult Eliminar(grupoModel ogrupo)
        {

            var respuesta = _grupoDatos.Eliminar(ogrupo.id);
            if (respuesta)

                return RedirectToAction("Listar");
            else
                return View();
        }

        public IActionResult ListarMenus(int idGrupo)
        {
            var Menus = _grupoDatos.ObtenerMenusPorGrupo(idGrupo);
            var viewModel = new MenusViewModel
            {
                Id = idGrupo,
                Menus = Menus
            };
            return View(viewModel);
        }

        public IActionResult ListarVehiculo(int idGrupo) 
        {
            var Lugares = _grupoDatos.ObtenerVehiculoPorGrupo(idGrupo);
            var viewModel = new VehiculoViewModel
            {
                Id = idGrupo,
                Lugares = Lugares
            };
            return View(viewModel);
        }
        public IActionResult ListarTipoActividad(int idGrupo)
        {
            var TipoActividad = _grupoDatos.ObtenerTipoActividadPorGrupo(idGrupo);
            var viewModel = new TipoActividadViewModel
            {
                Id = idGrupo,
                tipActividades = TipoActividad
            };
            return View(viewModel);
        }

    }

}
