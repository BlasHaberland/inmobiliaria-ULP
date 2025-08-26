using MySql.Data.MySqlClient;

namespace inmobiliaria.Data
{
    public static class Conexion
    {
        public static MySqlConnection ObtenerConexion(String connectionString)
        {
            var conexion = new MySqlConnection(connectionString);
            conexion.Open();
            return conexion;
        }
    }
}
