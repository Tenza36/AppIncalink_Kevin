using AppIncalink.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
namespace AppIncalink.Datos
{
    public class actividadesDatos
    {
        private readonly IWebHostEnvironment _env;

        public actividadesDatos(IWebHostEnvironment env)
        {
            _env = env;
        }
        //Metodo Listar
        public List<nombreActividadesModel> listar()
        {
            var oLista = new List<nombreActividadesModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ListarActividadesNombres", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new nombreActividadesModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            nombre = dr["nombre"].ToString(),
                            nombreGrupo = dr["nombreGrupo"].ToString(),
                            fechaInicio = Convert.ToDateTime(dr["fechaInicio"]),
                            fechaFin = Convert.ToDateTime(dr["fechaFin"]),
                            recursos = dr["recursos"].ToString(),
                            responsables = dr["responsable"].ToString(),
                            lugarDesde = dr["lugarDesde"].ToString(),
                            LugarHacia = dr["LugarHacia"].ToString(),
                            observaciones = dr["observaciones"].ToString(),
                            nombreTipoActividad = dr["nombreTipoActividad"].ToString(),
                            nombreMenu = dr["nombreMenu"].ToString(),
                            nombreVehiculo = dr["nombreVehiculo"].ToString()
                        });
                    }
                }
            }

            return oLista;
        }

        public actividadesModel Obtener(int id)
        {
            var oactividades = new actividadesModel();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ObtenerActividad", conexion);
                cmd.Parameters.AddWithValue("@idActividad", id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        oactividades.id = Convert.ToInt32(dr["id"]);
                        oactividades.nombre = dr["nombre"].ToString();
                        oactividades.idGrupo = Convert.ToInt32(dr["idGrupo"]);
                        oactividades.fechaInicio = Convert.ToDateTime(dr["fechaInicio"]);
                        oactividades.fechaFin = Convert.ToDateTime(dr["fechaFin"]);
                        oactividades.recursos = dr["recursos"].ToString();
                        oactividades.responsables = dr["responsable"].ToString();
                        oactividades.lugarDesde = dr["lugarDesde"].ToString();
                        oactividades.LugarHacia = dr["LugarHacia"].ToString();
                        oactividades.observaciones = dr["observaciones"].ToString();
                        oactividades.idTipoActividad = DBNull.Value.Equals(dr["idTipoActividad"]) ? 0 : Convert.ToInt32(dr["idTipoActividad"]);
                        oactividades.idMenu = DBNull.Value.Equals(dr["idMenu"]) ? 0 : Convert.ToInt32(dr["idMenu"]);
                        oactividades.idVehiculo = DBNull.Value.Equals(dr["idVehiculo"]) ? 0 : Convert.ToInt32(dr["idVehiculo"]);

                    }
                }
            }

            return oactividades;
        }


        //Metodo Guardar
        public bool Guardar(actividadesModel oactividades)
        {
            bool rpta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("InsertarActividad", conexion);
                    cmd.Parameters.AddWithValue("@nombre", oactividades.nombre);
                    cmd.Parameters.AddWithValue("@idGrupo", oactividades.idGrupo);
                    cmd.Parameters.AddWithValue("@fechaInicio", oactividades.fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", oactividades.fechaFin);
                    cmd.Parameters.AddWithValue("@recursos", oactividades.recursos);
                    cmd.Parameters.AddWithValue("@responsable", oactividades.responsables);
                    cmd.Parameters.AddWithValue("@lugarDesde", oactividades.lugarDesde);
                    cmd.Parameters.AddWithValue("@lugarHacia", oactividades.LugarHacia);
                    cmd.Parameters.AddWithValue("@observaciones", oactividades.observaciones);
                    cmd.Parameters.AddWithValue("@idTipoActividad", oactividades.idTipoActividad ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@idMenu", oactividades.idMenu ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@idVehiculo", oactividades.idVehiculo ?? (object)DBNull.Value);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                rpta = true;
            }
            catch (Exception e)
            {
                string error = e.Message;

                rpta = false;
            }

            return rpta;
        }



        //Metodo eliminar
        public bool Eliminar(int id)
        {
            bool rpta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();

                    // Verificar si la actividad con el ID proporcionado existe antes de eliminarla
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM actividades WHERE id = @id", conexion);
                    checkCmd.Parameters.AddWithValue("@id", id);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // La actividad existe, proceder con la eliminación
                        SqlCommand cmd = new SqlCommand("EliminarActividades", conexion);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        rpta = true;
                    }
                    else
                    {
                        // La actividad con el ID proporcionado no existe, establecer rpta como falso
                        rpta = false;
                    }
                }
            }
            catch (Exception e)
            {
                string error = e.Message;
                rpta = false;
            }

            return rpta;
        }


        
        public MemoryStream GeneratePdf(List<nombreActividadesModel> actividades)
        {
            var pdfStream = new MemoryStream();
            // Obtener la ruta absoluta de la imagen
            var imagePath = Path.Combine(_env.WebRootPath, "img", "Inca-Link-Ecuador_SQ_White-1.png");
            byte[] imageData = File.ReadAllBytes(imagePath);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                   
                    page.Margin(2, Unit.Centimetre);

                    page.Header()
                        .Element(element =>
                        {
                            element.Row(row =>
                            {
                                row.RelativeColumn()
                                    .Element(imageContainer =>
                                    {
                                        imageContainer
                                            .Height(100) // Ajusta la altura del contenedor
                                            .Width(100) // Ajusta el ancho del contenedor si es necesario
                                            .Image(imageData, ImageScaling.FitArea); // Ajusta la imagen dentro del contenedor
                                    });

                                row.RelativeColumn()
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text("Listado de Actividades")
                                    .FontSize(20)
                                    .Bold();
                            });
                        });

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Nombre").Bold();
                                header.Cell().Text("Grupo").Bold();
                                header.Cell().Text("Fecha Inicio").Bold();
                                header.Cell().Text("Fecha Fin").Bold();
                                header.Cell().Text("Recursos").Bold();
                                header.Cell().Text("Responsables").Bold();
                                header.Cell().Text("Observaciones").Bold();
                            });

                            foreach (var actividad in actividades)
                            {
                                table.Cell().Text(actividad.nombre);
                                table.Cell().Text(actividad.nombreGrupo);
                                table.Cell().Text(actividad.fechaInicio.ToString("dd/MM/yyyy"));
                                table.Cell().Text(actividad.fechaFin.ToString("dd/MM/yyyy"));
                                table.Cell().Text(actividad.recursos);
                                table.Cell().Text(actividad.responsables);
                                table.Cell().Text(actividad.observaciones);
                            }
                        });

                    page.Footer()
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                            text.Span(" of ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf(pdfStream);

            pdfStream.Position = 0;
            return pdfStream;
        }



    }
}
