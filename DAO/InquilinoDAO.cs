using inmobiliaria.Data;
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.DAO
{
    public class InquilinoDAO
    {
        private readonly string _connectionString;

        public InquilinoDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Inquilino> ObtenerTodos()
        {
            var lista = new List<Inquilino>();
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM inquilinos", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(mapearInquilino(reader));
            }
            return lista;
        }

        public Inquilino? obtenerPorId(int id)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM inquilinos WHERE id_inquilino = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return mapearInquilino(reader);
            }
            return null;
        }


        public bool crearInquilino(Inquilino inquilino)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("INSERT INTO inquilinos (dni, nombre, apellido, telefono, email, direccion, activo) VALUES (@dni, @nombre, @apellido, @telefono, @email, @direccion, @activo)", conexion);
            cmd.Parameters.AddWithValue("@dni", inquilino.Dni);
            cmd.Parameters.AddWithValue("@nombre", inquilino.Nombre);
            cmd.Parameters.AddWithValue("@apellido", inquilino.Apellido);
            cmd.Parameters.AddWithValue("@telefono", inquilino.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", inquilino.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@direccion", inquilino.Direccion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@activo", inquilino.Activo);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool actualizarInquilino(Inquilino inquilino)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE inquilinos SET dni = @dni, nombre = @nombre, apellido = @apellido, telefono = @telefono, email = @email, direccion = @direccion WHERE id_inquilino = @id", conexion);
            cmd.Parameters.AddWithValue("@id", inquilino.Id_Inquilino);
            cmd.Parameters.AddWithValue("@dni", inquilino.Dni);
            cmd.Parameters.AddWithValue("@nombre", inquilino.Nombre);
            cmd.Parameters.AddWithValue("@apellido", inquilino.Apellido);
            cmd.Parameters.AddWithValue("@telefono", inquilino.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", inquilino.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@direccion", inquilino.Direccion ?? (object)DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool eliminarInquilino(int id)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE inquilinos SET activo = 0 WHERE id_inquilino = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool altaInquilino(int id)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE inquilinos SET activo = 1 WHERE id_inquilino = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            if (cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Inquilino dado de alta exitosamente.");
                return true;
            }
            else
            {
                Console.WriteLine("No se pudo dar de alta el inquilino.");
                return false;
            }
        }

        private Inquilino mapearInquilino(MySqlDataReader reader)
        {
            return new Inquilino
            {
                Id_Inquilino = reader.GetInt32("id_inquilino"),
                Dni = reader.GetString("dni"),
                Nombre = reader.GetString("nombre"),
                Apellido = reader.GetString("apellido"),
                Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                Direccion = reader.IsDBNull(reader.GetOrdinal("direccion")) ? null : reader.GetString("direccion"),
                Activo = reader.GetBoolean("activo")
            };
        }
    }
}