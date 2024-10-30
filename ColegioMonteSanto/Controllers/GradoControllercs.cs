using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColegioMonteSanto.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GradosController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public GradosController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        // GET: /Grados/Listar
        // Permite que tanto el Administrador como el Profesor puedan listar los grados
        [HttpGet]
        [Route("Listar")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<IEnumerable<GradoModel>>> GetGrados()
        {
            return await _context.Grados.ToListAsync();
        }

        // GET: /Grados/{id}
        // Permite que tanto el Administrador como el Profesor puedan ver un grado específico
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<GradoModel>> GetGradoPorId(int id)
        {
            var grado = await _context.Grados.FindAsync(id);

            if (grado == null)
            {
                return NotFound("Grado no encontrado.");
            }

            return grado;
        }

        // POST: /Grados/Registrar
        // Solo el Administrador puede registrar nuevos grados
        [HttpPost]
        [Route("Registrar")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<GradoModel>> PostGrado(GradoModel grado)
        {
            _context.Grados.Add(grado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGradoPorId), new { id = grado.grado_id }, grado);
        }

        // PUT: /Grados/Editar/{id}
        // Solo el Administrador puede editar grados
        [HttpPut]
        [Route("Editar/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditarGrado(int id, [FromBody] GradoModel grado)
        {
            if (id != grado.grado_id)
            {
                return BadRequest("El ID del grado no coincide.");
            }

            if (grado == null)
            {
                return BadRequest("Se requiere un cuerpo de solicitud no vacío.");
            }

            _context.Entry(grado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradoExists(id))
                {
                    return NotFound("Grado no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: /Grados/Eliminar/{id}
        // Solo el Administrador puede eliminar grados
        [HttpDelete]
        [Route("Eliminar/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarGrado(int id)
        {
            var grado = await _context.Grados.FindAsync(id);

            if (grado == null)
            {
                return NotFound("Grado no encontrado.");
            }

            _context.Grados.Remove(grado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar si un grado existe
        private bool GradoExists(int id)
        {
            return _context.Grados.Any(e => e.grado_id == id);
        }
    }
}
