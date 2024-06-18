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
    public class reportesDatos
    {
        public List<comprasModel> ListaCompras(int idGrupo)
        {
            var oLista = new List<comprasModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ListarComprasPorGrupo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idGrupo", idGrupo);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new comprasModel()
                        {
                            nombreGrupo = dr["nombreGrupo"].ToString(),
                            nombreProducto = dr["nombreProducto"].ToString(),
                            tipoMedida = dr["tipoMedida"].ToString(),
                            cantidad = (float)Convert.ToDecimal(dr["cantidadTotal"])
                        });

                    }
                }
            }

            return oLista;
        }
        public MemoryStream GenerateComprasPdf(List<comprasModel> compras)
        {
            var pdfStream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);

                    page.Header()
                        .Element(element =>
                        {
                            element.Row(row =>
                            {
                                row.RelativeColumn()
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text("Listado de Compras")
                                    .FontSize(20)
                                    .Bold();
                            });
                        });

                    page.Content()
                        .Table(table =>
                        {
                            // Definición de columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // Nombre del Grupo
                                columns.RelativeColumn(); // Nombre del Producto
                                columns.RelativeColumn(); // Tipo de Medida
                                columns.RelativeColumn(); // Cantidad
                            });

                            // Encabezado de la tabla
                            table.Header(header =>
                            {
                                var headerCells = new string[]
                                {
                            "Nombre del Grupo", "Nombre del Producto", "Tipo de Medida", "Cantidad"
                                };

                                foreach (var headerText in headerCells)
                                {
                                    header.Cell()
                                        .Background(Colors.Grey.Darken3) // Cambia el color de fondo a un gris más oscuro
                                        .Padding(5)
                                        .AlignCenter() // Alinea el texto al centro
                                        .Text(headerText)
                                        .Bold()
                                        .FontColor(Colors.White);
                                }
                            });

                            // Filas de la tabla con datos de compras
                            foreach (var compra in compras)
                            {
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(compra.nombreGrupo));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(compra.nombreProducto));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(compra.tipoMedida));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(compra.cantidad.ToString()));
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf(pdfStream);

            pdfStream.Position = 0;
            return pdfStream;
        }



        //Obtner menus por grupo


        public List<MenuPorGrupoModel> ObtenerMenusPorGrupo(int idGrupo)
        {
            var oLista = new List<MenuPorGrupoModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerMenusPorGrupo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idGrupo", idGrupo);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new MenuPorGrupoModel()
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Nombre = dr["nombre"].ToString(),
                            NombreGrupo = dr["nombreGrupo"].ToString(),
                            FechaInicio = Convert.ToDateTime(dr["fechaInicio"]),
                            FechaFin = Convert.ToDateTime(dr["fechaFin"]),
                            Recursos = dr["recursos"].ToString(),
                            Responsables = dr["responsable"].ToString(),
                            LugarDesde = dr["lugarDesde"].ToString(),
                            LugarHacia = dr["lugarHacia"].ToString(),
                            Observaciones = dr["observaciones"].ToString(),
                            NombreMenu = dr["nombreMenu"].ToString()
                        });
                    }
                }
            }
            return oLista;
        }

        public MemoryStream GenerateMenuPdf(List<MenuPorGrupoModel> actividades)
        {
            var pdfStream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);

                    page.Header()
                        .Element(element =>
                        {
                            element.Row(row =>
                            {
                                row.RelativeColumn()
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text("Listado de Actividades por Grupo")
                                    .FontSize(20)
                                    .Bold();
                            });
                        });

                    page.Content()
                        .Table(table =>
                        {
                            // Definición de columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // Nombre de la Actividad
                                columns.RelativeColumn(); // Nombre del Grupo
                                columns.RelativeColumn(); // Fecha de Inicio
                                columns.RelativeColumn(); // Fecha Final
                                columns.RelativeColumn(); // Recursos
                                columns.RelativeColumn(); // Responsables
                                columns.RelativeColumn(); // Lugar Desde
                                columns.RelativeColumn(); // Lugar Hacia
                                columns.RelativeColumn(); // Observaciones
                                columns.RelativeColumn(); // Menú
                            });

                            // Encabezado de la tabla
                            table.Header(header =>
                            {
                                var headerCells = new string[]
                                {
                            "Nombre de la Actividad", "Nombre del Grupo", "Fecha de Inicio", "Fecha Final", "Recursos", "Responsable", "Lugar Desde", "Lugar Hacia", "Observaciones", "Menú"
                                };

                                foreach (var headerText in headerCells)
                                {
                                    header.Cell()
                                        .Background(Colors.Grey.Darken3) // Cambia el color de fondo a un gris más oscuro
                                        .Padding(5)
                                        .AlignCenter() // Alinea el texto al centro
                                        .Text(headerText)
                                        .Bold()
                                        .FontColor(Colors.White);
                                }
                            });

                            // Filas de la tabla con datos de actividades
                            foreach (var actividad in actividades)
                            {
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.Nombre));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.NombreGrupo));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.FechaInicio.ToString("dd/MM/ HH:mm"))); // Incluir hora
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.FechaFin.ToString("dd/MM/ HH:mm"))); // Incluir hora
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.Recursos));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.Responsables));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.LugarDesde));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.LugarHacia));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.Observaciones));
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(5)
                                    .AlignCenter() // Alinea el texto al centro
                                    .Element(CellText => CellText.Text(actividad.NombreMenu));
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf(pdfStream);

            pdfStream.Position = 0;
            return pdfStream;
        }


    }
}
