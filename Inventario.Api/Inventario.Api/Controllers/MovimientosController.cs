using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly InventarioDBContext _context;

        public MovimientosController(InventarioDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoInventario>>> GetMovimientos()
        {
            var movimientos = await _context.MovimientosInventario
                .AsNoTracking()
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
            return Ok(movimientos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoInventario>> GetMovimiento(int id)
        {
            var movimiento = await _context.MovimientosInventario
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdMovimiento == id);

            if (movimiento == null)
            {
                return NotFound(new { mensaje = $"Movimiento con ID {id} no encontrado." });
            }

            return Ok(movimiento);
        }

        [HttpGet("producto/{idProducto}")]
        public async Task<ActionResult<IEnumerable<MovimientoInventario>>> GetMovimientosPorProducto(int idProducto)
        {
            var movimientos = await _context.MovimientosInventario
                .AsNoTracking()
                .Where(m => m.IdProducto == idProducto)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();

            return Ok(movimientos);
        }

        [HttpPost]
        public async Task<ActionResult<MovimientoInventario>> CrearMovimiento([FromBody] MovimientoInventario movimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que la bodega exista
            var existeBodega = await _context.Bodegas.AnyAsync(b => b.IdBodega == movimiento.IdBodega);
            if (!existeBodega)
            {
                return BadRequest(new { mensaje = $"La bodega con ID {movimiento.IdBodega} no existe." });
            }

            if (movimiento.Cantidad <= 0)
            {
                return BadRequest(new { mensaje = "La cantidad del movimiento debe ser mayor a 0." });
            }

            var tipo = movimiento.Tipo.Trim().ToUpper();
            if (tipo != "ENTRADA" && tipo != "SALIDA")
            {
                return BadRequest(new { mensaje = "El tipo de movimiento debe ser 'ENTRADA' o 'SALIDA'." });
            }
            movimiento.Tipo = tipo;

            // Actualizar o crear existencia automáticamente
            var existencia = await _context.Existencias
                .FirstOrDefaultAsync(e => e.IdProducto == movimiento.IdProducto && e.IdBodega == movimiento.IdBodega);

            if (tipo == "ENTRADA")
            {
                if (existencia == null)
                {
                    existencia = new Existencia
                    {
                        IdProducto = movimiento.IdProducto,
                        IdBodega = movimiento.IdBodega,
                        Stock = movimiento.Cantidad
                    };
                    _context.Existencias.Add(existencia);
                }
                else
                {
                    existencia.Stock += movimiento.Cantidad;
                }
            }
            else // SALIDA
            {
                if (existencia == null || existencia.Stock < movimiento.Cantidad)
                {
                    var stockDisponible = existencia?.Stock ?? 0;
                    return BadRequest(new { mensaje = $"Stock insuficiente en la bodega {movimiento.IdBodega}. Stock disponible: {stockDisponible}, Solicitado: {movimiento.Cantidad}." });
                }
                existencia.Stock -= movimiento.Cantidad;
            }

            _context.MovimientosInventario.Add(movimiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.IdMovimiento }, movimiento);
        }
    }
}

