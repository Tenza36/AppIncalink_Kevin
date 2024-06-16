using AppIncalink.Models;
using System.Data;
using System.Data.SqlClient;
namespace AppIncalink.Datos
{
    public class platosDatos
    {
        public List<platonombreModel> listar()
        {
            var oLista = new List<platonombreModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ListarPlatosNombre", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new platonombreModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            nombre = dr["nombre"].ToString(),
                            nombreMenu = dr["nombreMenu"].ToString()
                        });
                    }
                }
            }

            return oLista;
        }

        public platosModel Obtener(int id)
        {
            var oplatos = new platosModel();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("ObtenerPlato", conexion);
                cmd.Parameters.AddWithValue("idPlatos", id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        oplatos.id = Convert.ToInt32(dr["id"]);
                        oplatos.nombre = dr["nombre"].ToString();
                        oplatos.idMenu = Convert.ToInt32(dr["idMenu"]);

                    }
                }
            }

            return oplatos;
        }
        //Metodo de guardar 
        public bool Guardar(platosModel oplatos)
        {
            bool rpta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("InsertarPlato", conexion);
                    cmd.Parameters.AddWithValue("@Nombre", oplatos.nombre);
                    cmd.Parameters.AddWithValue("@idMenu", oplatos.idMenu);

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
        public bool Editar(platosModel oplatos)
        {
            bool rpta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();


                    SqlCommand cmd = new SqlCommand("EditarPlato", conexion);
                    cmd.Parameters.AddWithValue("@id", oplatos.id); 
                    cmd.Parameters.AddWithValue("@nombre", oplatos.nombre);
                    cmd.Parameters.AddWithValue("@idMenu", oplatos.idMenu);
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

                    SqlCommand cmd = new SqlCommand("EliminarPlato", conexion);
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

        public List<recetasPorMenu> ListarRecetas(int idMenu) //recetas por plato 
        {
            var lista = new List<recetasPorMenu>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ListarRecetasPorPlato", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idMenu", idMenu);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new recetasPorMenu()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            nombrePlato = dr["nombrePlato"].ToString(),
                            nombreProducto = dr["nombreProducto"].ToString(),
                            cantidad = Convert.ToDecimal(dr["cantidad"])
                        });
                    }

                }
            }

            return lista;
        }
    }
}
