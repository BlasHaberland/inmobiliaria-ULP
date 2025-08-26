using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    public class Inquilino
    {
        [Key]
        [Column("id_inquilino")]
        public int Id_Inquilino { get; set; }

        [Column("dni")]
        [StringLength(20)]
        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string Dni { get; set; }

        [Column("nombre")]
        [StringLength(100)]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Column("apellido")]
        [StringLength(100)]
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Column("telefono")]
        [StringLength(50)]
        public string? Telefono { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("direccion")]
        [StringLength(255)]
        public string? Direccion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

    }
}
