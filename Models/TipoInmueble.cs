using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    public class TipoInmueble
    {
        [Column("id_tipo")]
        [Key]
        public int Id_Tipo { get; set; }

        [Column("nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string? Nombre { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;
    }
}