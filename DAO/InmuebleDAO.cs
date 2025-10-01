using System.Data;
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.DAO
{
    public class InmuebleDAO(string connectionString)
    {
        private readonly string _connectionString = connectionString;


        public List<Inmueble> ObtenerTodos()
        {
            var lista = new List<Inmueble>();
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM inmuebles", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearInmueble(reader));
            }
            return lista;
        }

        public List<Inmueble> ObtenerActivos()
        {
            var lista = new List<Inmueble>();
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM inmuebles WHERE activo = 1", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearInmueble(reader));
            }
            return lista;
        }

        public List<Inmueble> ObtenerDisponibles()
        {
            var lista = new List<Inmueble>();
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM inmuebles WHERE activo = 1 AND estado = 'disponible'", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearInmueble(reader));
            }
            return lista;
        }


        //TODO: Unificar ObtenerPorEstado con todos los Obterner
        public List<Inmueble> ObtenerPorEstado(string estado)
        {
            var lista = new List<Inmueble>();
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var query = "SELECT * FROM inmuebles";

            if (!string.IsNullOrEmpty(estado))
            {
                query += " WHERE estado = @estado";
            }

            var cmd = new MySqlCommand(query, conexion);

            if (!string.IsNullOrEmpty(estado))
            {
                cmd.Parameters.AddWithValue("@estado", estado);
            }

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(MapearInmueble(reader));
            }
            return lista;
        }

        public Inmueble? ObtenerPorId(int id)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM inmuebles WHERE id_inmueble = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapearInmueble(reader);
            }
            return null;
        }

        public List<Inmueble> ObtenerPorIdPropietario(int idPropietario)
        {
            var lista = new List<Inmueble>();
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM inmuebles WHERE id_propietario = @idPropietario", conexion);
            cmd.Parameters.AddWithValue("@idPropietario", idPropietario);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearInmueble(reader));
            }
            return lista;
        }

        public bool Crear(Inmueble inmueble)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);

            var cmd = new MySqlCommand("INSERT INTO inmuebles (id_propietario, id_tipo, uso, direccion, cantidad_ambientes, coordenadas, precio, estado, activo) VALUES (@id_propietario, @id_tipo, @uso, @direccion, @cantidad_ambientes, @coordenadas, @precio, @estado, 1)", conexion);

            cmd.Parameters.AddWithValue("@id_propietario", inmueble.Id_Propietario);
            cmd.Parameters.AddWithValue("@id_tipo", inmueble.Id_Tipo);
            cmd.Parameters.AddWithValue("@uso", inmueble.Uso);
            cmd.Parameters.AddWithValue("@direccion", inmueble.Direccion);
            cmd.Parameters.AddWithValue("@cantidad_ambientes", inmueble.Cantidad_Ambientes);
            cmd.Parameters.AddWithValue("@coordenadas", inmueble.Coordenadas ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@precio", inmueble.Precio);
            cmd.Parameters.AddWithValue("@estado", inmueble.Estado);

            return cmd.ExecuteNonQuery() > 0;

        }

        public bool Actualizar(Inmueble inmueble)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE inmuebles SET id_propietario = @id_propietario, id_tipo = @id_tipo, uso = @uso, direccion = @direccion, cantidad_ambientes = @cantidad_ambientes, coordenadas = @coordenadas, precio = @precio, estado = @estado WHERE id_inmueble = @id", conexion);
            cmd.Parameters.AddWithValue("@id", inmueble.Id_Inmueble);
            cmd.Parameters.AddWithValue("@id_propietario", inmueble.Id_Propietario);
            cmd.Parameters.AddWithValue("@id_tipo", inmueble.Id_Tipo);
            cmd.Parameters.AddWithValue("@uso", inmueble.Uso);
            cmd.Parameters.AddWithValue("@direccion", inmueble.Direccion);
            cmd.Parameters.AddWithValue("@cantidad_ambientes", inmueble.Cantidad_Ambientes);
            cmd.Parameters.AddWithValue("@coordenadas", inmueble.Coordenadas ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@precio", inmueble.Precio);
            cmd.Parameters.AddWithValue("@estado", inmueble.Estado);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool CambiarEstado(int idInmueble, string nuevoEstado)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE inmuebles SET estado = @estado WHERE id_inmueble = @id", conexion);
            cmd.Parameters.AddWithValue("@estado", nuevoEstado);
            cmd.Parameters.AddWithValue("@id", idInmueble);

            return cmd.ExecuteNonQuery() > 0;
        }
        public bool Eliminar(int id)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE inmuebles SET activo = 0, estado = 'suspendido' WHERE id_inmueble = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Alta(int id)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE inmuebles SET activo = 1, estado = 'disponible' WHERE id_inmueble = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        //AUXILIAR
        private Inmueble MapearInmueble(IDataRecord reader)
        {
            return new Inmueble
            {
                Id_Inmueble = reader.GetInt32(reader.GetOrdinal("id_inmueble")),
                Id_Propietario = reader.GetInt32(reader.GetOrdinal("id_propietario")),
                Id_Tipo = reader.GetInt32(reader.GetOrdinal("id_tipo")),
                Uso = reader.GetString(reader.GetOrdinal("uso")),
                Direccion = reader.GetString(reader.GetOrdinal("direccion")),
                Cantidad_Ambientes = reader.GetInt32(reader.GetOrdinal("cantidad_ambientes")),
                Coordenadas = reader.IsDBNull(reader.GetOrdinal("coordenadas")) ? null : reader.GetString(reader.GetOrdinal("coordenadas")),
                Precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                Estado = reader.GetString(reader.GetOrdinal("estado")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
    }
}