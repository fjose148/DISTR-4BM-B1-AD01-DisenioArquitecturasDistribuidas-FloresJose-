using Catalogo.Api.Data;
using Catalogo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.Api.Controllers
{
    [Route("api/[controller]")]
    [Route("api/producto")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly CatalogoDBContext _context;

        public ProductosController(CatalogoDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _context.Productos
                .AsNoTracking()
                .ToListAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
            {
                return NotFound(new { mensaje = $"Producto con ID {id} no encontrado." });
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto([FromBody] Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que la categoría exista
            var existeCategoria = await _context.Categorias.AnyAsync(c => c.IdCategoria == producto.IdCategoria);
            if (!existeCategoria)
            {
                return BadRequest(new { mensaje = $"La categoría con ID {producto.IdCategoria} no existe." });
            }

            // Validar que la marca exista
            var existeMarca = await _context.Marcas.AnyAsync(m => m.IdMarca == producto.IdMarca);
            if (!existeMarca)
            {
                return BadRequest(new { mensaje = $"La marca con ID {producto.IdMarca} no existe." });
            }

            // Validar SKU único
            var existeSku = await _context.Productos.AnyAsync(p => p.SKU == producto.SKU);
            if (existeSku)
            {
                return BadRequest(new { mensaje = $"El SKU '{producto.SKU}' ya se encuentra registrado." });
            }

            // Validar Precio
            if (producto.Precio < 0)
            {
                return BadRequest(new { mensaje = "El precio del producto debe ser mayor o igual a 0." });
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.IdProducto }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con el ID de la entidad." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que el producto exista
            var productoExistente = await _context.Productos.AnyAsync(p => p.IdProducto == id);
            if (!productoExistente)
            {
                return NotFound(new { mensaje = $"Producto con ID {id} no encontrado." });
            }

            // Validar que la categoría exista
            var existeCategoria = await _context.Categorias.AnyAsync(c => c.IdCategoria == producto.IdCategoria);
            if (!existeCategoria)
            {
                return BadRequest(new { mensaje = $"La categoría con ID {producto.IdCategoria} no existe." });
            }

            // Validar que la marca exista
            var existeMarca = await _context.Marcas.AnyAsync(m => m.IdMarca == producto.IdMarca);
            if (!existeMarca)
            {
                return BadRequest(new { mensaje = $"La marca con ID {producto.IdMarca} no existe." });
            }

            // Validar SKU único en otro producto
            var existeSku = await _context.Productos.AnyAsync(p => p.SKU == producto.SKU && p.IdProducto != id);
            if (existeSku)
            {
                return BadRequest(new { mensaje = $"El SKU '{producto.SKU}' ya está en uso por otro producto." });
            }

            // Validar Precio
            if (producto.Precio < 0)
            {
                return BadRequest(new { mensaje = "El precio del producto debe ser mayor o igual a 0." });
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Productos.AnyAsync(p => p.IdProducto == id))
                {
                    return NotFound(new { mensaje = $"Producto con ID {id} no encontrado." });
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = $"Producto con ID {id} no encontrado." });
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

