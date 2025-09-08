using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    public class Inmueble
    {
        [Key]
        [Column("id_inmueble")]
        public int Id_Inmueble { get; set; }

        [Column("id_propietario")]
        [Required(ErrorMessage = "El propietario es obligatorio")]
        public int Id_Propietario { get; set; }

        [Column("id_tipo")]
        [Required(ErrorMessage = "El tipo de inmueble es obligatorio")]
        public int Id_Tipo { get; set; }

        [Column("uso")]
        [Required(ErrorMessage = "El uso es obligatorio")]
        public string? Uso { get; set; } // ENUM (Residencial, Comercial)

        [Column("direccion")]
        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(255)]
        public string? Direccion { get; set; }

        [Column("cantidad_ambientes")]
        public int Cantidad_Ambientes { get; set; }

        [Column("coordenadas")]
        [StringLength(100)]
        public string? Coordenadas { get; set; }

        [Column("precio")]
        [Required(ErrorMessage = "El precio es obligatorio")]
        public decimal Precio { get; set; }

        [Column("estado")]
        public string? Estado { get; set; } // ENUM (Disponible, Suspendido, Ocupado)

        [Column("activo")]
        public bool Activo { get; set; }

    }
}