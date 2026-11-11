using Pedidos.Api.Data;
using Pedidos.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Api.Controllers
{
    [Route("api/[controller]")]
    [Route("api/pedido")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly PedidosDBContext _context;

        public PedidosController(PedidosDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .AsNoTracking()
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoConDetallesDto>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound(new { mensaje = $"Pedido con ID {id} no encontrado." });
            }

            var detalles = await _context.DetallesPedido
                .AsNoTracking()
                .Where(d => d.IdPedido == id)
                .ToListAsync();

            var resultado = new PedidoConDetallesDto
            {
                IdPedido = pedido.IdPedido,
                IdCliente = pedido.IdCliente,
                Fecha = pedido.Fecha,
                Total = pedido.Total,
                Estado = pedido.Estado,
                Detalles = detalles
            };

            return Ok(resultado);
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosPorCliente(int idCliente)
        {
            var pedidos = await _context.Pedidos
                .AsNoTracking()
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoConDetallesDto>> CrearPedido([FromBody] CrearPedidoDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que el cliente exista físicamente
            var existeCliente = await _context.Clientes.AnyAsync(c => c.IdCliente == request.IdCliente);
            if (!existeCliente)
            {
                return BadRequest(new { mensaje = $"El cliente con ID {request.IdCliente} no existe." });
            }

            if (request.Detalles == null || !request.Detalles.Any())
            {
                return BadRequest(new { mensaje = "El pedido debe tener al menos un detalle de producto." });
            }

            // Validar cantidades y precios en detalles
            foreach (var item in request.Detalles)
            {
                if (item.Cantidad <= 0)
                {
                    return BadRequest(new { mensaje = "La cantidad de cada producto debe ser mayor a 0." });
                }
                if (item.PrecioUnitario < 0)
                {
                    return BadRequest(new { mensaje = "El precio unitario no puede ser negativo." });
                }
            }

            // Calcular total
            var total = request.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

            var nuevoPedido = new Pedido
            {
                IdCliente = request.IdCliente,
                Fecha = DateTime.Now,
                Total = total,
                Estado = "PENDIENTE"
            };

            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            var detallesEntities = request.Detalles.Select(d => new DetallePedido
            {
                IdPedido = nuevoPedido.IdPedido,
                IdProducto = d.IdProducto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList();

            _context.DetallesPedido.AddRange(detallesEntities);
            await _context.SaveChangesAsync();

            var resultado = new PedidoConDetallesDto
            {
                IdPedido = nuevoPedido.IdPedido,
                IdCliente = nuevoPedido.IdCliente,
                Fecha = nuevoPedido.Fecha,
                Total = nuevoPedido.Total,
                Estado = nuevoPedido.Estado,
                Detalles = detallesEntities
            };

            return CreatedAtAction(nameof(GetPedido), new { id = nuevoPedido.IdPedido }, resultado);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoPedido(int id, [FromQuery] string nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound(new { mensaje = $"Pedido con ID {id} no encontrado." });
            }

            var estadoUpper = nuevoEstado?.Trim().ToUpper();
            if (estadoUpper != "PENDIENTE" && estadoUpper != "COMPLETADO" && estadoUpper != "CANCELADO")
            {
                return BadRequest(new { mensaje = "El estado debe ser 'PENDIENTE', 'COMPLETADO' o 'CANCELADO'." });
            }

            pedido.Estado = estadoUpper;
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound(new { mensaje = $"Pedido con ID {id} no encontrado." });
            }

            var detalles = _context.DetallesPedido.Where(d => d.IdPedido == id);
            _context.DetallesPedido.RemoveRange(detalles);
            _context.Pedidos.Remove(pedido);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

