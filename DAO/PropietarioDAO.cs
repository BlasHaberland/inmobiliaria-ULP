using inmobiliaria.Data;
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.DAO
{
    public class PropietarioDAO(string connectionString)
    {
        private readonly string _connectionString = connectionString;


        //METODOS
        public List<Propietario> obtenerTodos()
        {
            var lista = new List<Propietario>();
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM propietarios", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearPropietario(reader));
            }
            return lista;
        }

        public int contarPropietarios()
        {
            int total = 0;
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM propietarios", conexion);
            total = Convert.ToInt32(cmd.ExecuteScalar());
            Console.WriteLine("Total de propietarios: " + total);
            return total;
        }

        public List<Propietario> obtenerPaginados(int pagina, int tamanoPagina)
        {
            var propietarios = new List<Propietario>();
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM propietarios LIMIT @limite OFFSET @offset", conexion);
            cmd.Parameters.AddWithValue("@limite", tamanoPagina);
            cmd.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                propietarios.Add(MapearPropietario(reader));
            }
            return propietarios;
        }


        public List<Propietario> obtenerActivos()
        {
            var lista = new List<Propietario>();
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM propietarios WHERE activo = 1", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearPropietario(reader));
            }
            return lista;
        }

        public Propietario? obtenerPorId(int id)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM propietarios WHERE id_propietario = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapearPropietario(reader);
            }

            return null;
        }

        public bool crearPropietario(Propietario propietario)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("INSERT INTO propietarios (dni, nombre, apellido, telefono, email, direccion, activo) VALUES (@dni, @nombre, @apellido, @telefono, @email, @direccion, @activo)", conexion);
            cmd.Parameters.AddWithValue("@dni", propietario.Dni);
            cmd.Parameters.AddWithValue("@nombre", propietario.Nombre);
            cmd.Parameters.AddWithValue("@apellido", propietario.Apellido);
            cmd.Parameters.AddWithValue("@telefono", propietario.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", propietario.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@direccion", propietario.Direccion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@activo", propietario.Activo);

            if (cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Propietario creado exitosamente.");
                return true;
            }
            else
            {
                Console.WriteLine("No se pudo crear el propietario.");
                return false;
            }
        }

        public bool actualizarPropietario(Propietario propietario)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE propietarios SET dni = @dni, nombre = @nombre, apellido = @apellido, telefono = @telefono, email = @email, direccion = @direccion WHERE id_propietario = @id", conexion);
            cmd.Parameters.AddWithValue("@id", propietario.Id_Propietario);
            cmd.Parameters.AddWithValue("@dni", propietario.Dni);
            cmd.Parameters.AddWithValue("@nombre", propietario.Nombre);
            cmd.Parameters.AddWithValue("@apellido", propietario.Apellido);
            cmd.Parameters.AddWithValue("@telefono", propietario.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", propietario.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@direccion", propietario.Direccion ?? (object)DBNull.Value);

            if (cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Propietario actualizado exitosamente." + propietario);

                return true;
            }
            else
            {
                Console.WriteLine("No se pudo actualizado el propietario.");
                return false;
            }
        }

        public bool eliminarPropietario(int id)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE propietarios SET activo = 0 WHERE id_propietario = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            if (cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Propietario eliminado exitosamente.");
                return true;
            }
            else
            {
                Console.WriteLine("No se pudo eliminar el propietario.");
                return false;
            }
        }

        public bool altaPropietario(int id)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE propietarios SET activo = 1 WHERE id_propietario = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            if (cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Propietario dado de alta exitosamente.");
                return true;
            }
            else
            {
                Console.WriteLine("No se pudo dar de alta el propietario.");
                return false;
            }

        }
        private Propietario MapearPropietario(MySqlDataReader reader)
        {
            return new Propietario
            {
                Id_Propietario = reader.GetInt32("id_propietario"),
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