using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    public class Contrato
    {
        [Key]
        [Column("id_contrato")]
        public int Id_Contrato { get; set; }

        [Required(ErrorMessage = "El inquilino es obligatorio")]
        [Column("id_inquilino")]
        public int Id_Inquilino { get; set; }

        [Required(ErrorMessage = "El inmueble es obligatorio")]
        [Column("id_inmueble")]
        public int Id_Inmueble { get; set; }

        [Required(ErrorMessage = "El usuario creador es obligatorio")]
        [Column("id_usuario_creador")]
        public int Id_Usuario_Creador { get; set; }

        [Column("id_usuario_finalizador")]
        public int? Id_Usuario_Finalizador { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [Column("fecha_inicio")]
        public DateTime Fecha_Inicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [Column("fecha_fin_original")]
        public DateTime Fecha_Fin_Original { get; set; }

        [Column("fecha_fin_anticipada")]
        public DateTime Fecha_Fin_Anticipada { get; set; }


        [Required(ErrorMessage = "El monto mensual es obligatorio")]
        [Column("monto_mensual", TypeName = "decimal(12,2)")]
        public decimal Monto_Mensual { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("multa", TypeName = "decimal(12,2)")]

        public decimal Multa { get; set; }

        //auxiliares
        public string? Nombre_Inquilino { get; set; }
        public string? Apellido_Inquilino { get; set; }

        //TO STRING COMPLETO
        public override string ToString()
        {
            return $"Contrato [Id_Contrato={Id_Contrato}, Id_Inquilino={Id_Inquilino}, Id_Inmueble={Id_Inmueble}, Id_Usuario_Creador={Id_Usuario_Creador}, Id_Usuario_Finalizador={Id_Usuario_Finalizador}, Fecha_Inicio={Fecha_Inicio}, Fecha_Fin_Original={Fecha_Fin_Original}, Fecha_Fin_Anticipada={Fecha_Fin_Anticipada}, Monto_Mensual={Monto_Mensual}, Estado={Estado}, Multa={Multa}, Nombre_Inquilino={Nombre_Inquilino}, Apellido_Inquilino={Apellido_Inquilino}]";
        }

    }


}