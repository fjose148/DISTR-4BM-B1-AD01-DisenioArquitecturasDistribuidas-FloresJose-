using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models
{
    [Table("DetallePedido")]
    public class DetallePedido
    {
        [Key]
        [Column("IdDetalle")]
        public int IdDetalle { get; set; }

        [Required]
        [Column("IdPedido")]
        public int IdPedido { get; set; } // FK física a Pedido

        [Required]
        [Column("IdProducto")]
        public int IdProducto { get; set; } // Referencia lógica a CatalogoDB.Producto

        [Required]
        [Column("Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required]
        [Column("PrecioUnitario", TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor o igual a 0.")]
        public decimal PrecioUnitario { get; set; }
    }
}

