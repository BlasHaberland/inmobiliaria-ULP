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

        public bool Actualizar(Pago pago)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand(@"UPDATE pagos SET id_contrato = @id_contrato, numero_pago = @numero_pago, fecha_vencimiento = @fecha_vencimiento, fecha_pago = @fecha_pago, detalle = @detalle, importe = @importe, estado = @estado, id_usuario_creador = @id_usuario_creador, id_usuario_anulador = @id_usuario_anulador WHERE id_pago = @id_pago", conexion);

            cmd.Parameters.AddWithValue("@id_pago", pago.Id_Pago);
            cmd.Parameters.AddWithValue("@id_contrato", pago.Id_Contrato);
            cmd.Parameters.AddWithValue("@numero_pago", pago.Numero_Pago);
            cmd.Parameters.AddWithValue("@fecha_vencimiento", pago.Fecha_Vencimiento);
            cmd.Parameters.AddWithValue("@fecha_pago", (object?)pago.Fecha_Pago ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@detalle", (object?)pago.Detalle ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@importe", pago.Importe);
            cmd.Parameters.AddWithValue("@estado", (object?)pago.Estado ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id_usuario_creador", pago.Id_Usuario_Creador);
            cmd.Parameters.AddWithValue("@id_usuario_anulador", (object?)pago.Id_Usuario_Anulador ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;

        }

        public bool AgregarPago(Pago pago)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand(@"INSERT INTO pagos 
        (id_contrato, numero_pago, fecha_vencimiento, fecha_pago, detalle, importe, estado, id_usuario_creador, id_usuario_anulador) 
        VALUES (@id_contrato, @numero_pago, @fecha_vencimiento, @fecha_pago, @detalle, @importe, @estado, @id_usuario_creador, @id_usuario_anulador)", conexion);

            cmd.Parameters.AddWithValue("@id_contrato", pago.Id_Contrato);
            cmd.Parameters.AddWithValue("@numero_pago", pago.Numero_Pago);
            cmd.Parameters.AddWithValue("@fecha_vencimiento", pago.Fecha_Vencimiento);
            cmd.Parameters.AddWithValue("@fecha_pago", (object?)pago.Fecha_Pago ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@detalle", (object?)pago.Detalle ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@importe", pago.Importe);
            cmd.Parameters.AddWithValue("@estado", (object?)pago.Estado ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id_usuario_creador", pago.Id_Usuario_Creador);
            cmd.Parameters.AddWithValue("@id_usuario_anulador", (object?)pago.Id_Usuario_Anulador ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
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