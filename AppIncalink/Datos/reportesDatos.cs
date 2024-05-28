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
                                        .Background(Colors.Grey.Lighten2)
                                        .Padding(5)
                                        .AlignCenter()
                                        .Text(headerText)
                                        .Bold()
                                        .FontColor(Colors.White);
                                }
                            });

                            // Filas de la tabla con datos de compras
                            foreach (var compra in compras)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(5).Element(CellText => CellText.Text(compra.nombreGrupo));
                                table.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(5).Element(CellText => CellText.Text(compra.nombreProducto));
                                table.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(5).Element(CellText => CellText.Text(compra.tipoMedida));
                                table.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(5).Element(CellText => CellText.Text(compra.cantidad.ToString()));
                            }
                        });

                    page.Footer()
                        .AlignCenter()
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
