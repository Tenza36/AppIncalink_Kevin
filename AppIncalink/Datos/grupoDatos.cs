﻿using AppIncalink.Models;
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
    public class grupoDatos
    {
       
        //Metodo Listar
        public List<grupoModel> listar()
        {
            var oLista = new List<grupoModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ListarGrupos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new grupoModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            nombre = dr["nombre"].ToString(),
                            fechaLlegada = Convert.ToDateTime(dr["fechaLlegada"]),
                            fechaSalida = Convert.ToDateTime(dr["fechaSalida"]),
                            informacionVuelo = dr["informacionVuelo"].ToString(),
                            intinerarioVuelo = dr["intinerarioVuelo"].ToString(),
                            observaciones = dr["observaciones"].ToString()
                        });
                    }
                }
            }

            return oLista;
        }

        //Metodo obtener datos

        public grupoModel Obtener(int id)
        {
            var ogrupo = new grupoModel();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ObtenerGrupo", conexion);
                cmd.Parameters.AddWithValue("idGrupo", id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        ogrupo.id = Convert.ToInt32(dr["id"]);
                        ogrupo.nombre = dr["nombre"].ToString();
                        ogrupo.fechaLlegada = Convert.ToDateTime(dr["fechaLlegada"]);
                        ogrupo.fechaSalida = Convert.ToDateTime(dr["fechaSalida"]);
                        ogrupo.informacionVuelo = dr["informacionVuelo"].ToString();
                        ogrupo.intinerarioVuelo = dr["intinerarioVuelo"].ToString();
                        ogrupo.observaciones = dr["observaciones"].ToString();
                    }
                }
            }

            return ogrupo;
        }

        //Metodo de guardar 
        public bool Guardar(grupoModel ogrupo)
        {
            bool rpta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("InsertarGrupo", conexion);
                    cmd.Parameters.AddWithValue("@Nombre", ogrupo.nombre);
                    cmd.Parameters.AddWithValue("@FechaLlegada", ogrupo.fechaLlegada);
                    cmd.Parameters.AddWithValue("@FechaSalida", ogrupo.fechaSalida);
                    cmd.Parameters.AddWithValue("@InformacionVuelo", ogrupo.informacionVuelo);
                    cmd.Parameters.AddWithValue("@IntinerarioVuelo", ogrupo.intinerarioVuelo);
                    cmd.Parameters.AddWithValue("@Observaciones", ogrupo.observaciones);
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
        //Metodo editar 
        public bool Editar(grupoModel ogrupo)
        {
            bool rpta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("EditarGrupo", conexion); // Ajusta el nombre del procedimiento almacenado
                    cmd.Parameters.AddWithValue("@id", ogrupo.id);
                    cmd.Parameters.AddWithValue("@nombre", ogrupo.nombre);
                    cmd.Parameters.AddWithValue("@fechaLlegada", ogrupo.fechaLlegada);
                    cmd.Parameters.AddWithValue("@fechaSalida", ogrupo.fechaSalida);
                    cmd.Parameters.AddWithValue("@informacionVuelo", ogrupo.informacionVuelo);
                    cmd.Parameters.AddWithValue("@intinerarioVuelo", ogrupo.intinerarioVuelo);
                    cmd.Parameters.AddWithValue("@observaciones", ogrupo.observaciones);
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

                    SqlCommand cmd = new SqlCommand("EliminarGrupo", conexion);
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

        public List<ListarPersonasNombresModel> ObtenerPersonasPorGrupo(int idGrupo)
        {
            var oLista = new List<ListarPersonasNombresModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerPersonasPorGrupo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idGrupo", idGrupo);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new ListarPersonasNombresModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            nombreCompleto = dr["nombreCompleto"].ToString(),
                            documentoId = dr["documentoId"].ToString(),
                            correo = dr["correo"].ToString(),
                            telefono = dr["telefono"].ToString(),
                            alergiaAlimentacion = dr["alergiaAlimentacion"].ToString(),
                            alegiaVarias = dr["alegiaVarias"].ToString(),
                            observaciones = dr["observaciones"].ToString(),
                            numeroHabitacion = dr["numeroHabitacion"].ToString(),
                            nombreGrupo = dr["nombreGrupo"].ToString(),
                            nombreRol = dr["nombreRol"].ToString(),
                            nombreSexo = dr["nombreSexo"].ToString()
                        });
                    }
                }
            }

            return oLista;
        }

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

        //Obtener vehiculo por grupo
        public List<MenuPorVehiculoModel> ObtenerVehiculoPorGrupo(int idGrupo)
        {
            var oLista = new List<MenuPorVehiculoModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerVehiculoPorGrupo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idGrupo", idGrupo);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new MenuPorVehiculoModel()
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
                            Lugar = dr["nombreVehiculo"].ToString()
                        });
                    }
                }
            }
            return oLista;
        }


        //Obtener vehiculo por grupo
        public List<tipoActividadPorGrupoModel> ObtenerTipoActividadPorGrupo(int idGrupo)
        {
            var oLista = new List<tipoActividadPorGrupoModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerTipoActividadPorGrupo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idGrupo", idGrupo);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new tipoActividadPorGrupoModel()
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
                            tipoActividad = dr["nombreTipoActividad"].ToString()
                        });
                    }
                }
            }
            return oLista;
        }

    }
}
