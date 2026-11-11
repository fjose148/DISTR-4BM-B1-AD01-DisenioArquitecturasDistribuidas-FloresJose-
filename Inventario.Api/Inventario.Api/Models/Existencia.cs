using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Api.Models
{
    [Table("Existencia")]
    public class Existencia
    {
        [Key]
        [Column("IdExistencia")]
        public int IdExistencia { get; set; }

        [Required]
        [Column("IdProducto")]
        public int IdProducto { get; set; } // Referencia lógica a CatalogoDB.Producto

        [Required]
        [Column("IdBodega")]
        public int IdBodega { get; set; } // FK física a Bodega

        [Required]
        [Column("Stock")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0.")]
        public int Stock { get; set; }
    }
}

