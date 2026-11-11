using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Api.Controllers
{
    [Route("api/[controller]")]
    [Route("api/bodega")]
    [ApiController]
    public class BodegasController : ControllerBase
    {
        private readonly InventarioDBContext _context;

        public BodegasController(InventarioDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bodega>>> GetBodegas()
        {
            var bodegas = await _context.Bodegas
                .AsNoTracking()
                .ToListAsync();
            return Ok(bodegas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bodega>> GetBodega(int id)
        {
            var bodega = await _context.Bodegas
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.IdBodega == id);

            if (bodega == null)
            {
                return NotFound(new { mensaje = $"Bodega con ID {id} no encontrada." });
            }

            return Ok(bodega);
        }

        [HttpPost]
        public async Task<ActionResult<Bodega>> CrearBodega([FromBody] Bodega bodega)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Bodegas.Add(bodega);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBodega), new { id = bodega.IdBodega }, bodega);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarBodega(int id, [FromBody] Bodega bodega)
        {
            if (id != bodega.IdBodega)
            {
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con el ID de la bodega." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(bodega).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Bodegas.AnyAsync(b => b.IdBodega == id))
                {
                    return NotFound(new { mensaje = $"Bodega con ID {id} no encontrada." });
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarBodega(int id)
        {
            var bodega = await _context.Bodegas.FindAsync(id);
            if (bodega == null)
            {
                return NotFound(new { mensaje = $"Bodega con ID {id} no encontrada." });
            }

            _context.Bodegas.Remove(bodega);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

