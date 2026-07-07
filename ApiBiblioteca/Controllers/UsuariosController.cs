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
    public class UsuariosController : ControllerBase
    {
        private readonly bibliotecaContext _context;

        public UsuariosController(bibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<usuarios>>> Getusuarios()
        {
            return await _context.usuarios.Where(u=>u.estado ==true).ToListAsync();
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<usuarios>> Getusuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<usuarios>> Postusuario(usuarios usuario)
        {
            _context.usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Getusuario), new { id = usuario.id_usuario }, usuario);
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putusuario(int id, usuarios usuario)
        {
            if (id != usuario.id_usuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
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

        // DELETE: api/usuarios/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Deleteusuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // 2. Validar si está siendo usado
            bool estaEnUso = await _context.prestamos.AnyAsync(re => re.id_usuario == id);

            if (estaEnUso)
            {
                // 409 Conflict indica que la acción no se puede procesar debido a un conflicto de estado
                return Conflict(new { message = "No se puede eliminar el registro porque está siendo utilizado actualmente." });
            }

            usuario.estado = false;
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

        private bool usuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.id_usuario == id);
        }
    }
}
