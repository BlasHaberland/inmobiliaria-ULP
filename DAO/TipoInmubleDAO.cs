using System.Data;
using inmobiliaria.Models;
using MySql.Data.MySqlClient;


namespace inmobiliaria.DAO
{
    public class TipoInmuebleDAO(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public List<TipoInmueble> ObtenerTodos()
        {
            var lista = new List<TipoInmueble>();
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM tipos_inmueble", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearTipoInmueble(reader));
            }
            return lista;
        }

        public TipoInmueble? ObtenerPorId(int id)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM tipos_inmueble WHERE id_tipo = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapearTipoInmueble(reader);
            }
            return null;
        }

        public bool Crear(TipoInmueble tipoInmueble)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("INSERT INTO tipos_inmueble (nombre, descripcion) VALUES (@nombre, @descripcion)", conexion);
            cmd.Parameters.AddWithValue("@nombre", tipoInmueble.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", tipoInmueble.Descripcion);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Actualizar(TipoInmueble tipoInmueble)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE tipos_inmueble SET nombre = @nombre, descripcion = @descripcion, activo = @activo WHERE id_tipo = @id", conexion);
            cmd.Parameters.AddWithValue("@id", tipoInmueble.Id_Tipo);
            cmd.Parameters.AddWithValue("@nombre", tipoInmueble.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", tipoInmueble.Descripcion);
            cmd.Parameters.AddWithValue("@activo", tipoInmueble.Activo);
            return cmd.ExecuteNonQuery() > 0;
        }

        //AUXILIAR
        private TipoInmueble MapearTipoInmueble(MySqlDataReader reader)
        {
            return new TipoInmueble
            {
                Id_Tipo = Convert.ToInt32(reader["id_tipo"]),
                Nombre = reader["nombre"].ToString() ?? string.Empty,
                Descripcion = reader["descripcion"].ToString() ?? string.Empty,
                Activo = Convert.ToBoolean(reader["activo"])
            };
        }
    }
}