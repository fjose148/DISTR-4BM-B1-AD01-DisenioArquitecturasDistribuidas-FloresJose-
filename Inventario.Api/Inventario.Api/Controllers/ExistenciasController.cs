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
    [Route("api/existencia")]
    [ApiController]
    public class ExistenciasController : ControllerBase
    {
        private readonly InventarioDBContext _context;

        public ExistenciasController(InventarioDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Existencia>>> GetExistencias()
        {
            var existencias = await _context.Existencias
                .AsNoTracking()
                .ToListAsync();
            return Ok(existencias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Existencia>> GetExistencia(int id)
        {
            var existencia = await _context.Existencias
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.IdExistencia == id);

            if (existencia == null)
            {
                return NotFound(new { mensaje = $"Registro de existencia con ID {id} no encontrado." });
            }

            return Ok(existencia);
        }

        [HttpGet("producto/{idProducto}")]
        public async Task<ActionResult<IEnumerable<Existencia>>> GetExistenciasPorProducto(int idProducto)
        {
            var existencias = await _context.Existencias
                .AsNoTracking()
                .Where(e => e.IdProducto == idProducto)
                .ToListAsync();

            return Ok(existencias);
        }

        [HttpGet("bodega/{idBodega}")]
        public async Task<ActionResult<IEnumerable<Existencia>>> GetExistenciasPorBodega(int idBodega)
        {
            var existencias = await _context.Existencias
                .AsNoTracking()
                .Where(e => e.IdBodega == idBodega)
                .ToListAsync();

            return Ok(existencias);
        }

        [HttpPost]
        public async Task<ActionResult<Existencia>> CrearExistencia([FromBody] Existencia existencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que la bodega exista físicamente
            var existeBodega = await _context.Bodegas.AnyAsync(b => b.IdBodega == existencia.IdBodega);
            if (!existeBodega)
            {
                return BadRequest(new { mensaje = $"La bodega con ID {existencia.IdBodega} no existe." });
            }

            if (existencia.Stock < 0)
            {
                return BadRequest(new { mensaje = "El stock debe ser mayor o igual a 0." });
            }

            _context.Existencias.Add(existencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExistencia), new { id = existencia.IdExistencia }, existencia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarExistencia(int id, [FromBody] Existencia existencia)
        {
            if (id != existencia.IdExistencia)
            {
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con el ID de la existencia." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existeBodega = await _context.Bodegas.AnyAsync(b => b.IdBodega == existencia.IdBodega);
            if (!existeBodega)
            {
                return BadRequest(new { mensaje = $"La bodega con ID {existencia.IdBodega} no existe." });
            }

            if (existencia.Stock < 0)
            {
                return BadRequest(new { mensaje = "El stock debe ser mayor o igual a 0." });
            }

            _context.Entry(existencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Existencias.AnyAsync(e => e.IdExistencia == id))
                {
                    return NotFound(new { mensaje = $"Existencia con ID {id} no encontrada." });
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarExistencia(int id)
        {
            var existencia = await _context.Existencias.FindAsync(id);
            if (existencia == null)
            {
                return NotFound(new { mensaje = $"Existencia con ID {id} no encontrada." });
            }

            _context.Existencias.Remove(existencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

