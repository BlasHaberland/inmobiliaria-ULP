using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    public class Pago
    {
        [Key]
        [Column("id_pago")]
        public int Id_Pago { get; set; }

        [Required]
        [Column("id_contrato")]
        public int Id_Contrato { get; set; }

        [Required]
        [Column("numero_pago")]
        public int Numero_Pago { get; set; }

        [Required]
        [Column("fecha_vencimiento")]
        [DataType(DataType.Date)]
        public DateTime Fecha_Vencimiento { get; set; }

        [Column("fecha_pago")]
        [DataType(DataType.Date)]
        public DateTime? Fecha_Pago { get; set; }

        [Column("detalle")]
        [StringLength(255)]
        public string? Detalle { get; set; }

        [Required]
        [Column("importe", TypeName = "decimal(12,2)")]
        public decimal Importe { get; set; }

        [Required]
        [Column("estado")]
        [StringLength(15)]
        public string? Estado { get; set; }

        [Required]
        [Column("id_usuario_creador")]
        public int Id_Usuario_Creador { get; set; }

        [Column("id_usuario_anulador")]
        public int? Id_Usuario_Anulador { get; set; }
    }
}