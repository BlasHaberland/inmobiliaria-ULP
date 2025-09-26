using System.Data;
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.DAO
{
    public class ContratoDAO(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public List<Contrato> ObtenerTodos()
        {
            var lista = new List<Contrato>();
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySql.Data.MySqlClient.MySqlCommand(@"SELECT c.*, i.nombre AS Nombre_Inquilino, i.apellido AS Apellido_Inquilino FROM contratos c JOIN inquilinos i ON c.id_inquilino = i.id_inquilino", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearContrato(reader));
            }
            return lista;
        }

        public Contrato? ObtenerPorId(int id)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand(@"SELECT c.*, i.nombre AS Nombre_Inquilino, i.apellido AS Apellido_Inquilino FROM contratos c JOIN inquilinos i ON c.id_inquilino = i.id_inquilino WHERE c.id_contrato = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapearContrato(reader);
            }
            return null;
        }

        public bool Crear(Contrato contrato)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            using var transaccion = conexion.BeginTransaction();

            try
            {
                var cmd = new MySqlCommand("INSERT INTO contratos (id_inquilino, id_inmueble, fecha_fin_original,fecha_inicio, monto_mensual, id_usuario_creador, estado, multa) VALUES (@id_inquilino, @id_inmueble, @fecha_fin_original,@fecha_inicio, @monto_mensual, @id_usuario_creador, @estado, @multa)", conexion);

                cmd.Parameters.AddWithValue("@id_inquilino", contrato.Id_Inquilino);
                cmd.Parameters.AddWithValue("@id_inmueble", contrato.Id_Inmueble);
                cmd.Parameters.AddWithValue("@fecha_fin_original", contrato.Fecha_Fin_Original);
                cmd.Parameters.AddWithValue("@fecha_inicio", contrato.Fecha_Inicio);
                cmd.Parameters.AddWithValue("@monto_mensual", contrato.Monto_Mensual);
                cmd.Parameters.AddWithValue("@id_usuario_creador", contrato.Id_Usuario_Creador);
                cmd.Parameters.AddWithValue("@estado", contrato.Estado ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@multa", contrato.Multa);

                cmd.ExecuteNonQuery();

                //Obtengo el ID generado
                var idCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conexion);
                var id = Convert.ToInt32(idCmd.ExecuteScalar());

                //Calculo los pagos
                DateTime fechaInicio = contrato.Fecha_Inicio;
                DateTime fechaFin = contrato.Fecha_Fin_Original;
                int cantidadPagos = ((fechaFin.Year - fechaInicio.Year) * 12) + fechaFin.Month - fechaInicio.Month + 1;

                //Creo los pagos
                for (int i = 0; i < cantidadPagos; i++)
                {
                    DateTime fechaVencimiento = fechaInicio.AddMonths(i);
                    var pagoCmd = new MySqlCommand("INSERT INTO pagos (id_contrato, numero_pago, fecha_vencimiento, importe, estado, id_usuario_creador) VALUES (@id_contrato, @numero_pago, @fecha_vencimiento, @importe, @estado, @id_usuario_creador)", conexion);
                    pagoCmd.Parameters.AddWithValue("@id_contrato", id);
                    pagoCmd.Parameters.AddWithValue("@numero_pago", i + 1);
                    pagoCmd.Parameters.AddWithValue("@fecha_vencimiento", fechaVencimiento);
                    pagoCmd.Parameters.AddWithValue("@importe", contrato.Monto_Mensual);
                    pagoCmd.Parameters.AddWithValue("@estado", "pendiente");
                    pagoCmd.Parameters.AddWithValue("@id_usuario_creador", contrato.Id_Usuario_Creador);

                    pagoCmd.ExecuteNonQuery();

                }
                transaccion.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                throw new Exception("Error al crear contrato: " + ex.Message, ex);
            }
        }

        public bool Actualizar(Contrato contrato)
        {
            using var conexion = Data.Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("UPDATE contratos SET id_inquilino = @id_inquilino, id_inmueble = @id_inmueble, fecha_fin_original = @fecha_fin_original, fecha_fin_anticipada = @fecha_fin_anticipada, monto_mensual = @monto_mensual, id_usuario_finalizador = @id_usuario_finalizador, estado = @estado, multa = @multa WHERE id_contrato = @id_contrato", conexion);
            cmd.Parameters.AddWithValue("@id_inquilino", contrato.Id_Inquilino);
            cmd.Parameters.AddWithValue("@id_inmueble", contrato.Id_Inmueble);
            cmd.Parameters.AddWithValue("@fecha_fin_original", contrato.Fecha_Fin_Original);
            cmd.Parameters.AddWithValue("@fecha_fin_anticipada", contrato.Fecha_Fin_Anticipada != DateTime.MinValue ? contrato.Fecha_Fin_Anticipada : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@monto_mensual", contrato.Monto_Mensual);
            cmd.Parameters.AddWithValue("@id_usuario_finalizador", contrato.Id_Usuario_Finalizador != null ? contrato.Id_Usuario_Finalizador : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@estado", contrato.Estado != null ? contrato.Estado : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@multa", contrato.Multa);
            cmd.Parameters.AddWithValue("@id_contrato", contrato.Id_Contrato);

            int filasAfectadas = cmd.ExecuteNonQuery();
            return filasAfectadas > 0;
        }


        //AUXILIAR
        private Contrato MapearContrato(IDataRecord record)
        {
            return new Contrato
            {
                Id_Contrato = Convert.ToInt32(record["id_contrato"]),
                Id_Inquilino = Convert.ToInt32(record["id_inquilino"]),
                Id_Inmueble = Convert.ToInt32(record["id_inmueble"]),
                Id_Usuario_Creador = Convert.ToInt32(record["id_usuario_creador"]),
                Id_Usuario_Finalizador = record["id_usuario_finalizador"] != DBNull.Value ? Convert.ToInt32(record["id_usuario_finalizador"]) : (int?)null,
                Fecha_Fin_Original = Convert.ToDateTime(record["fecha_fin_original"]),
                Fecha_Fin_Anticipada = record["fecha_fin_anticipada"] != DBNull.Value ? Convert.ToDateTime(record["fecha_fin_anticipada"]) : DateTime.MinValue,
                Monto_Mensual = Convert.ToDecimal(record["monto_mensual"]),
                Estado = record["estado"] != DBNull.Value ? record["estado"].ToString() : null,
                Multa = record["multa"] != DBNull.Value ? Convert.ToDecimal(record["multa"]) : 0,
                Nombre_Inquilino = record["Nombre_Inquilino"] != DBNull.Value ? record["Nombre_Inquilino"].ToString() : null,
                Apellido_Inquilino = record["Apellido_Inquilino"] != DBNull.Value ? record["Apellido_Inquilino"].ToString() : null,
            };
        }
    }
}