using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        [Column("IdPedido")]
        public int IdPedido { get; set; }

        [Required]
        [Column("IdCliente")]
        public int IdCliente { get; set; } // FK física a Cliente

        [Column("Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column("Total", TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0.")]
        public decimal Total { get; set; } = 0.00m;

        [Required]
        [StringLength(50)]
        [Column("Estado")]
        public string Estado { get; set; } = "PENDIENTE"; // 'PENDIENTE', 'COMPLETADO', 'CANCELADO'
    }
}

