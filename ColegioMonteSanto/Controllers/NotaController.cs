using ColegioMonteSanto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ColegioMonteSanto.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ColegioMonteSanto.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotasController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public NotasController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<IEnumerable<NotaModel>>> GetNotas()
        {
            return await _context.Notas
                .Include(n => n.Alumno)
                .Include(n => n.Materia)
                .ToListAsync();
        }

        [HttpGet]
        [Route("ListarPropias")]
        [Authorize(Roles = "Alumno")]
        public async Task<ActionResult<IEnumerable<NotaModel>>> GetNotasPropias()
        {
            var alumnoIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(alumnoIdString))
            {
                return Unauthorized("No se pudo obtener el ID del alumno.");
            }

            var alumnoId = int.Parse(alumnoIdString);
            return await _context.Notas
                .Where(n => n.alumno_id == alumnoId)
                .Include(n => n.Materia)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador, Profesor, Alumno")]
        public async Task<ActionResult<NotaModel>> GetNotaPorId(int id)
        {
            var nota = await _context.Notas
                .Include(n => n.Alumno)
                .Include(n => n.Materia)
                .FirstOrDefaultAsync(n => n.nota_id == id);

            if (nota == null)
            {
                return NotFound("Nota no encontrada.");
            }

            if (User.IsInRole("Alumno"))
            {
                var alumnoIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(alumnoIdString))
                {
                    return Unauthorized("No se pudo obtener el ID del alumno.");
                }

                var alumnoId = int.Parse(alumnoIdString);
                if (nota.alumno_id != alumnoId)
                {
                    return Forbid("No tienes acceso a esta nota.");
                }
            }

            return nota;
        }

        [HttpPost]
        [Route("Registrar")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<NotaModel>> PostNota(NotaModel nota)
        {
            _context.Notas.Add(nota);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotaPorId", new { id = nota.nota_id }, nota);
        }

        [HttpPut]
        [Route("Editar/{id}")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<IActionResult> EditarNota(int id, [FromBody] NotaModel nota)
        {
            if (id != nota.nota_id)
            {
                return BadRequest("El ID de la nota no coincide.");
            }

            if (nota == null)
            {
                return BadRequest("Se requiere un cuerpo de solicitud no vacío.");
            }

            _context.Entry(nota).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotaExists(id))
                {
                    return NotFound("Nota no encontrada.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<IActionResult> EliminarNota(int id)
        {
            var nota = await _context.Notas.FindAsync(id);

            if (nota == null)
            {
                return NotFound("Nota no encontrada.");
            }

            _context.Notas.Remove(nota);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotaExists(int id)
        {
            return _context.Notas.Any(e => e.nota_id == id);
        }
    }
}
