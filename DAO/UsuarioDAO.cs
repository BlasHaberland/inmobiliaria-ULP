using System.Data;
using inmobiliaria.Data;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;

namespace inmobiliaria.DAO
{
    public class UsuarioDAO(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public bool CrearUsuario(Usuario usuario)
        {
            var hasher = new PasswordHasher<Usuario>();
            usuario.Contrasena = hasher.HashPassword(usuario, usuario.Contrasena);

            usuario.Nombre = Capitalizar(usuario.Nombre);
            usuario.Apellido = Capitalizar(usuario.Apellido);

            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand(@"INSERT INTO usuarios 
                    (email, contrasena, nombre, apellido, rol, avatar, activo) 
                    VALUES (@Email, @Contrasena, @Nombre, @Apellido, @Rol, @Avatar, @Activo)", conexion);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@Avatar", usuario.Avatar ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Activo", usuario.Activo);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Usuario? Login(string email, string contrasena)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM usuarios WHERE email = @Email AND activo = 1", conexion);
            cmd.Parameters.AddWithValue("@Email", email);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var usuario = MapearUsuario(reader);
                var hasher = new PasswordHasher<Usuario>();
                var resultado = hasher.VerifyHashedPassword(usuario, usuario.Contrasena, contrasena);
                if (resultado == PasswordVerificationResult.Success)
                {
                    return usuario;
                }
            }
            return null;
        }

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM usuarios", conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearUsuario(reader));
            }
            return lista;
        }

        public Usuario? ObtenerPorId(int id)
        {
            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand("SELECT * FROM usuarios WHERE id_usuario = @Id", conexion);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapearUsuario(reader);
            }
            return null;
        }
        public bool Actualizar(Usuario usuario)
        {
            usuario.Nombre = Capitalizar(usuario.Nombre);
            usuario.Apellido = Capitalizar(usuario.Apellido);

            var usuarioActual = ObtenerPorId(usuario.Id_Usuario);
            if (usuarioActual != null && usuario.Contrasena != usuarioActual.Contrasena)
            {
                var hasher = new PasswordHasher<Usuario>();
                usuario.Contrasena = hasher.HashPassword(usuario, usuario.Contrasena);
            }

            using var conexion = Conexion.ObtenerConexion(_connectionString);
            var cmd = new MySqlCommand(@"UPDATE usuarios SET 
                    email = @Email, 
                    contrasena = @Contrasena,
                    nombre = @Nombre, 
                    apellido = @Apellido, 
                    rol = @Rol, 
                    avatar = @Avatar, 
                    activo = @Activo 
                    WHERE id_usuario = @Id_Usuario", conexion);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@Avatar", usuario.Avatar ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Activo", usuario.Activo);
            cmd.Parameters.AddWithValue("@Id_Usuario", usuario.Id_Usuario);

            return cmd.ExecuteNonQuery() > 0;
        }

        // Auxiliares
        private static string Capitalizar(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;
            texto = texto.Trim().ToLower();
            return char.ToUpper(texto[0]) + texto.Substring(1);
        }

        private Usuario MapearUsuario(IDataRecord reader)
        {
            return new Usuario
            {
                Id_Usuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                Contrasena = reader.GetString(reader.GetOrdinal("contrasena")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                Rol = reader.GetString(reader.GetOrdinal("rol")),
                Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString(reader.GetOrdinal("avatar")),
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                FechaActualizacion = reader.GetDateTime(reader.GetOrdinal("fecha_actualizacion")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
    }
}