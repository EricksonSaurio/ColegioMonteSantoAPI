using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ColegioMonteSanto.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActividadesController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public ActividadesController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        [Authorize(Roles = "Administrador,Profesor,Estudiante")]
        public async Task<ActionResult<IEnumerable<ActividadModel>>> GetActividades()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Alumno")
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var actividadesAlumno = await _context.Notas
                    .Where(n => n.alumno_id == userId)
                    .Include(n => n.Actividad)
                    .Select(n => n.Actividad)
                    .Distinct()
                    .ToListAsync();

                return actividadesAlumno;
            }

            return await _context.Actividades.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador,Profesor,Estudiante")]
        public async Task<ActionResult<ActividadModel>> GetActividadPorId(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound("Actividad no encontrada.");
            }

            return actividad;
        }

        [HttpPost]
        [Route("Registrar")]
        [Authorize(Roles = "Administrador,Profesor")]
        public async Task<ActionResult<ActividadModel>> PostActividad(ActividadModel actividad)
        {
            _context.Actividades.Add(actividad);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActividadPorId), new { id = actividad.actividad_id }, actividad);
        }

        [HttpPut]
        [Route("Editar/{id}")]
        [Authorize(Roles = "Administrador,Profesor")]
        public async Task<IActionResult> EditarActividad(int id, [FromBody] ActividadModel actividad)
        {
            if (id != actividad.actividad_id)
            {
                return BadRequest("El ID de la actividad no coincide.");
            }

            _context.Entry(actividad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActividadExists(id))
                {
                    return NotFound("Actividad no encontrada.");
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
        [Authorize(Roles = "Administrador,Profesor")]
        public async Task<IActionResult> EliminarActividad(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound("Actividad no encontrada.");
            }

            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActividadExists(int id)
        {
            return _context.Actividades.Any(e => e.actividad_id == id);
        }
    }
}
