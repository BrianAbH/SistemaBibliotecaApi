using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseFirst.Contexts;
using DataBaseFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly bibliotecaContext _context;

        public CategoriasController(bibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<categorias>>> Getcategorias()
        {
            return await _context.categorias.ToListAsync();
        }

        // GET: api/categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<categorias>> Getcategoria(int id)
        {
            var categoria = await _context.categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<ActionResult<categorias>> Postcategoria(categorias categoria)
        {
            _context.categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Getcategoria), new { id = categoria.id_categoria }, categoria);
        }

        // PUT: api/categorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putcategoria(int id, categorias categoria)
        {
            if (id != categoria.id_categoria)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!categoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletecategoria(int id)
        {
            var categoria = await _context.categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.categorias.Remove(categoria);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al eliminar.");
            }

            return NoContent();
        }

        private bool categoriaExists(int id)
        {
            return _context.categorias.Any(e => e.id_categoria == id);
        }
    }
}
