using Pedidos.Api.Data;
using Pedidos.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesPedidoController : ControllerBase
    {
        private readonly PedidosDBContext _context;

        public DetallesPedidoController(PedidosDBContext context)
        {
            _context = context;
        }

        [HttpGet("pedido/{idPedido}")]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPorPedido(int idPedido)
        {
            var detalles = await _context.DetallesPedido
                .AsNoTracking()
                .Where(d => d.IdPedido == idPedido)
                .ToListAsync();

            return Ok(detalles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedido>> GetDetalle(int id)
        {
            var detalle = await _context.DetallesPedido
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDetalle == id);

            if (detalle == null)
            {
                return NotFound(new { mensaje = $"Detalle con ID {id} no encontrado." });
            }

            return Ok(detalle);
        }

        [HttpPost]
        public async Task<ActionResult<DetallePedido>> AgregarDetalle([FromBody] DetallePedido detalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pedido = await _context.Pedidos.FindAsync(detalle.IdPedido);
            if (pedido == null)
            {
                return BadRequest(new { mensaje = $"El pedido con ID {detalle.IdPedido} no existe." });
            }

            if (detalle.Cantidad <= 0)
            {
                return BadRequest(new { mensaje = "La cantidad debe ser mayor a 0." });
            }

            if (detalle.PrecioUnitario < 0)
            {
                return BadRequest(new { mensaje = "El precio unitario no puede ser negativo." });
            }

            _context.DetallesPedido.Add(detalle);

            // Recalcular total del pedido
            pedido.Total += (detalle.Cantidad * detalle.PrecioUnitario);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetalle), new { id = detalle.IdDetalle }, detalle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarDetalle(int id)
        {
            var detalle = await _context.DetallesPedido.FindAsync(id);
            if (detalle == null)
            {
                return NotFound(new { mensaje = $"Detalle con ID {id} no encontrado." });
            }

            var pedido = await _context.Pedidos.FindAsync(detalle.IdPedido);
            if (pedido != null)
            {
                pedido.Total -= (detalle.Cantidad * detalle.PrecioUnitario);
                if (pedido.Total < 0) pedido.Total = 0;
            }

            _context.DetallesPedido.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

