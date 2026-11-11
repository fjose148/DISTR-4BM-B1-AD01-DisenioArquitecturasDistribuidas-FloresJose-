using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Api.Models
{
    [Table("ReservaStock")]
    public class ReservaStock
    {
        [Key]
        [Column("IdReserva")]
        public int IdReserva { get; set; }

        [Required]
        [Column("IdProducto")]
        public int IdProducto { get; set; } // Referencia lógica a CatalogoDB.Producto

        [Required]
        [Column("IdBodega")]
        public int IdBodega { get; set; } // FK física a Bodega

        [Required]
        [Column("Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Column("Estado")]
        [StringLength(50)]
        public string Estado { get; set; } = "PENDIENTE"; // 'PENDIENTE', 'CONFIRMADA', 'CANCELADA'
    }
}

