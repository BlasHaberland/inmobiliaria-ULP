using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int Id_Usuario { get; set; }

        [Column("email")]
        [StringLength(100)]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; } = string.Empty;

        [Column("contrasena")]
        [StringLength(255)]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; } = string.Empty;

        [Column("nombre")]
        [StringLength(100)]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Column("apellido")]
        [StringLength(100)]
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; } = string.Empty;

        [Column("rol")]
        [Required]
        public string Rol { get; set; } = string.Empty;

        [Column("avatar")]
        [StringLength(255)]
        public string? Avatar { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

    }
}