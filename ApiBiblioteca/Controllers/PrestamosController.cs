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
    public class PrestamosController : ControllerBase
    {
        private readonly bibliotecaContext _context;

        public PrestamosController(bibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/prestamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<prestamos>>> Getprestamos()
        {
            return await _context.prestamos.ToListAsync();
        }

        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<prestamos>>> GetPrestamosActivos()
        {
            return await _context.prestamos.Where(p => p.estado == "En Préstamo").ToListAsync();
        }

        // GET: api/prestamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<prestamos>> Getprestamo(int id)
        {
            var prestamo = await _context.prestamos.FindAsync(id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return prestamo;
        }

        // POST: api/prestamos
        [HttpPost]
        public async Task<ActionResult<prestamos>> Postprestamo(prestamos prestamo)
        {
            _context.prestamos.Add(prestamo);
            var libro = await _context.libros.FindAsync(prestamo.id_libro);

            libro.ReducirEjemplares(1);


            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Getprestamo), new { id = prestamo.id_prestamo }, prestamo);
        }

        // PUT: api/prestamos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putprestamo(int id, prestamos prestamo)
        {
            if (id != prestamo.id_prestamo)
            {
                return BadRequest();
            }

            _context.Entry(prestamo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!prestamoExists(id))
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

        // PATCH: api/prestamos/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Deleteprestamo(int id)
        {
            var prestamo = await _context.prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            var libro = await _context.libros.FindAsync(prestamo.id_libro);
            libro.devolverEjemplares(1);


            prestamo.estado = "Devuelto";

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

        private bool prestamoExists(int id)
        {
            return _context.prestamos.Any(e => e.id_prestamo == id);
        }
    }
}
    