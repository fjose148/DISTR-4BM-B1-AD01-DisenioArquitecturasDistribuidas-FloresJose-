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
    public class ReservasController : ControllerBase
    {
        private readonly InventarioDBContext _context;

        public ReservasController(InventarioDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaStock>>> GetReservas()
        {
            var reservas = await _context.ReservasStock
                .AsNoTracking()
                .ToListAsync();
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaStock>> GetReserva(int id)
        {
            var reserva = await _context.ReservasStock
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdReserva == id);

            if (reserva == null)
            {
                return NotFound(new { mensaje = $"Reserva con ID {id} no encontrada." });
            }

            return Ok(reserva);
        }

        [HttpGet("producto/{idProducto}")]
        public async Task<ActionResult<IEnumerable<ReservaStock>>> GetReservasPorProducto(int idProducto)
        {
            var reservas = await _context.ReservasStock
                .AsNoTracking()
                .Where(r => r.IdProducto == idProducto)
                .ToListAsync();

            return Ok(reservas);
        }

        [HttpPost]
        public async Task<ActionResult<ReservaStock>> CrearReserva([FromBody] ReservaStock reserva)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existeBodega = await _context.Bodegas.AnyAsync(b => b.IdBodega == reserva.IdBodega);
            if (!existeBodega)
            {
                return BadRequest(new { mensaje = $"La bodega con ID {reserva.IdBodega} no existe." });
            }

            if (reserva.Cantidad <= 0)
            {
                return BadRequest(new { mensaje = "La cantidad a reservar debe ser mayor a 0." });
            }

            if (string.IsNullOrWhiteSpace(reserva.Estado))
            {
                reserva.Estado = "PENDIENTE";
            }

            _context.ReservasStock.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.IdReserva }, reserva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarReserva(int id, [FromBody] ReservaStock reserva)
        {
            if (id != reserva.IdReserva)
            {
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con el ID de la reserva." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existeBodega = await _context.Bodegas.AnyAsync(b => b.IdBodega == reserva.IdBodega);
            if (!existeBodega)
            {
                return BadRequest(new { mensaje = $"La bodega con ID {reserva.IdBodega} no existe." });
            }

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ReservasStock.AnyAsync(r => r.IdReserva == id))
                {
                    return NotFound(new { mensaje = $"Reserva con ID {id} no encontrada." });
                }
                throw;
            }

            return NoContent();
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoReserva(int id, [FromQuery] string nuevoEstado)
        {
            var reserva = await _context.ReservasStock.FindAsync(id);
            if (reserva == null)
            {
                return NotFound(new { mensaje = $"Reserva con ID {id} no encontrada." });
            }

            var estadoUpper = nuevoEstado?.Trim().ToUpper();
            if (estadoUpper != "PENDIENTE" && estadoUpper != "CONFIRMADA" && estadoUpper != "CANCELADA")
            {
                return BadRequest(new { mensaje = "El estado debe ser 'PENDIENTE', 'CONFIRMADA' o 'CANCELADA'." });
            }

            reserva.Estado = estadoUpper;
            await _context.SaveChangesAsync();

            return Ok(reserva);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarReserva(int id)
        {
            var reserva = await _context.ReservasStock.FindAsync(id);
            if (reserva == null)
            {
                return NotFound(new { mensaje = $"Reserva con ID {id} no encontrada." });
            }

            _context.ReservasStock.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

