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
    public class LibrosController : ControllerBase
    {
        private readonly bibliotecaContext _context;

        public LibrosController(bibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/libroes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<libros>>> Getlibros()
        {
            return await _context.libros.Where(l=>l.estado == true).ToListAsync();
        }

        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<libros>>> GetlibrosS()
        {
            return await _context.libros.Where(l => l.estado == true && l.ejemplares > 0).ToListAsync();
        }

        // GET: api/libroes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<libros>> Getlibro(int id)
        {
            var libro = await _context.libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        // POST: api/libroes
        [HttpPost]
        public async Task<ActionResult<libros>> Postlibro(libros libro)
        {
            _context.libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Getlibro), new { id = libro.id_libro }, libro);
        }

        // PUT: api/libroes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putlibro(int id, libros libro)
        {
            if (id != libro.id_libro)
            {
                return BadRequest();
            }

            _context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!libroExists(id))
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

        // DELETE: api/libroes/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Deletelibro(int id)
        {
            var libro = await _context.libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            bool estaEnUso = await _context.prestamos.AnyAsync(re => re.id_libro == id);

            if (estaEnUso)
            {
                return Conflict(new { message = "No se puede eliminar el registro porque está siendo utilizado actualmente." });
            }

            libro.estado = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar.");
            }
            

            return NoContent();
        }

        private bool libroExists(int id)
        {
            return _context.libros.Any(e => e.id_libro == id);
        }
    }
}
