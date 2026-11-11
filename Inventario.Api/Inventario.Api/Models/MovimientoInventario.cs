using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Api.Models
{
    [Table("MovimientoInventario")]
    public class MovimientoInventario
    {
        [Key]
        [Column("IdMovimiento")]
        public int IdMovimiento { get; set; }

        [Required]
        [Column("IdProducto")]
        public int IdProducto { get; set; } // Referencia lógica a CatalogoDB.Producto

        [Required]
        [Column("IdBodega")]
        public int IdBodega { get; set; } // FK física a Bodega

        [Required]
        [StringLength(50)]
        [Column("Tipo")]
        public string Tipo { get; set; } = string.Empty; // 'ENTRADA', 'SALIDA'

        [Required]
        [Column("Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}

