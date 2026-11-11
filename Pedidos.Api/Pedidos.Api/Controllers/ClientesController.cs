using Pedidos.Api.Data;
using Pedidos.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pedidos.Api.Controllers
{
    [Route("api/[controller]")]
    [Route("api/cliente")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly PedidosDBContext _context;

        public ClientesController(PedidosDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null)
            {
                return NotFound(new { mensaje = $"Cliente con ID {id} no encontrado." });
            }

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> CrearCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar cédula única
            var existeCedula = await _context.Clientes.AnyAsync(c => c.Cedula == cliente.Cedula);
            if (existeCedula)
            {
                return BadRequest(new { mensaje = $"La cédula '{cliente.Cedula}' ya se encuentra registrada." });
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con el ID del cliente." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existeCedula = await _context.Clientes.AnyAsync(c => c.Cedula == cliente.Cedula && c.IdCliente != id);
            if (existeCedula)
            {
                return BadRequest(new { mensaje = $"La cédula '{cliente.Cedula}' ya está en uso por otro cliente." });
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Clientes.AnyAsync(c => c.IdCliente == id))
                {
                    return NotFound(new { mensaje = $"Cliente con ID {id} no encontrado." });
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = $"Cliente con ID {id} no encontrado." });
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

