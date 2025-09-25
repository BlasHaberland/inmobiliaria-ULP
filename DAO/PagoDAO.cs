using inmobiliaria.Data;
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.DAO
{
    public class PagoDAO
    {
        private readonly string _connectionString;
        public PagoDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Pago ObtenerPorId(int idPago)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM pagos WHERE id_pago = @idPago", conexion);
            cmd.Parameters.AddWithValue("@idPago", idPago);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return mapearPago(reader);
            }
            return null;
        }

        public List<Pago> ObtenerPorContrato(int idContrato)
        {
            var lista = new List<Pago>();
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM pagos WHERE id_contrato = @idContrato", conexion);
            cmd.Parameters.AddWithValue("@idContrato", idContrato);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(mapearPago(reader));
            }
            return lista;
        }


        //AUXILIARES
        private Pago mapearPago(MySqlDataReader reader)
        {
            return new Pago
            {
                Id_Pago = reader.GetInt32("id_pago"),
                Id_Contrato = reader.GetInt32("id_contrato"),
                Numero_Pago = reader.GetInt32("numero_pago"),
                Fecha_Vencimiento = reader.GetDateTime("fecha_vencimiento"),
                Fecha_Pago = reader.IsDBNull(reader.GetOrdinal("fecha_pago")) ? (DateTime?)null : reader.GetDateTime("fecha_pago"),
                Detalle = reader.IsDBNull(reader.GetOrdinal("detalle")) ? null : reader.GetString("detalle"),
                Importe = reader.GetDecimal("importe"),
                Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? null : reader.GetString("estado"),
                Id_Usuario_Creador = reader.GetInt32("id_usuario_creador"),
                Id_Usuario_Anulador = reader.IsDBNull(reader.GetOrdinal("id_usuario_anulador")) ? (int?)null : reader.GetInt32("id_usuario_anulador")
            };
        }
    }

}