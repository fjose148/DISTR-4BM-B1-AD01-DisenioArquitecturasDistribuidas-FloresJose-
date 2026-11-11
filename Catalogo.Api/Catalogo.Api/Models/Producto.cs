using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalogo.Api.Models
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Required]
        [Column("IdCategoria")]
        public int IdCategoria { get; set; }

        [Required]
        [Column("IdMarca")]
        public int IdMarca { get; set; }

        [Required]
        [StringLength(150)]
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("SKU")]
        public string SKU { get; set; } = string.Empty;

        [Column("Precio", TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
        public decimal Precio { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; } = true;
    }
}

