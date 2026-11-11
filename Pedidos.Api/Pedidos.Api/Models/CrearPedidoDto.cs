using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pedidos.Api.Models
{
    public class CrearDetalleDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor o igual a 0.")]
        public decimal PrecioUnitario { get; set; }
    }

    public class CrearPedidoDto
    {
        [Required]
        public int IdCliente { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "El pedido debe contener al menos un detalle de producto.")]
        public List<CrearDetalleDto> Detalles { get; set; } = new();
    }

    public class PedidoConDetallesDto
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<DetallePedido> Detalles { get; set; } = new();
    }
}

