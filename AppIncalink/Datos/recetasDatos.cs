﻿using AppIncalink.Datos;
using AppIncalink.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AppIncalink.Datos
{
    public class recetasDatos
    {
        //Metodo Listar
        public List<recetasModel> listar()
        {
            var oLista = new List<recetasModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ListarRecetas", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new recetasModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            idPlato = Convert.ToInt32(dr["idPlato"]),
                            idProducto = Convert.ToInt32(dr["idProducto"]),
                            cantidad = Convert.ToDecimal(dr["cantidad"])
                        });
                    }
                }
            }

            return oLista;
        }

        public List<recetasPorMenu> listarNombre()
        {
            var oLista = new List<recetasPorMenu>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ListarRecetasNombre", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new recetasPorMenu()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            nombrePlato = dr["nombrePlato"].ToString(),
                            nombreProducto = dr["nombreProducto"].ToString(),
                            cantidad = Convert.ToDecimal(dr["cantidad"])
                        });
                    }
                }
            }

            return oLista;
        }

        //Metodo Obtener 
        public recetasModel Obtener(int id)
        {
            var orecetas = new recetasModel();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ObtenerRecetas", conexion);
                cmd.Parameters.AddWithValue("idRecetas", id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        orecetas.id = Convert.ToInt32(dr["id"]);
                        orecetas.idPlato = Convert.ToInt32(dr["idPlato"]);
                        orecetas.idProducto = Convert.ToInt32(dr["idProducto"]);
                        orecetas.cantidad = Convert.ToDecimal(dr["cantidad"]);
                    }
                }
            }

            return orecetas;
        }
        //Metodo Guardar
        public bool Guardar(recetasModel orecetas)
        {
            bool rpta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("InsertarRecetas", conexion);
                    cmd.Parameters.AddWithValue("@idPlato", orecetas.idPlato);
                    cmd.Parameters.AddWithValue("@idProducto", orecetas.idProducto);
                    cmd.Parameters.AddWithValue("@cantidad", orecetas.cantidad);
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

        // Recetas para el listado del reporte

        public List<recetasModel> ObtenerProductosPorReceta(int idReceta)
        {
            var productos = new List<recetasModel>();
            var cn = new Conexion();
            using (var connection = new SqlConnection(cn.getCadenaSQL()))
            {
                connection.Open();

                string query = "SELECT idProducto, cantidad FROM RecetaProducto WHERE idReceta = @IdReceta";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdReceta", idReceta);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var receta = new recetasModel
                            {
                                idProducto = Convert.ToInt32(reader["idProducto"]),
                                cantidad = Convert.ToDecimal(reader["cantidad"])
                            };

                            productos.Add(receta);
                        }
                    }
                }
            }

            return productos;
        }

        public bool Eliminar(int id)
        {
            bool rpta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("EliminarReceta", conexion);
                    cmd.Parameters.AddWithValue("@id", id);
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
    }
}