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

        // GET: /Actividades/Listar
        [HttpGet]
        [Route("Listar")]
        [Authorize(Roles = "Administrador,Profesor,Alumno")]
        public async Task<ActionResult<IEnumerable<ActividadModel>>> GetActividades()
        {
            // Obtén el rol del usuario logueado
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Alumno")
            {
                // Obtén el ID del usuario (alumno) logueado
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Filtra las actividades para mostrar solo las asociadas al alumno en la tabla notas
                var actividadesAlumno = await _context.Notas
                    .Where(n => n.alumno_id == userId)
                    .Include(n => n.Actividad)  // Incluye la actividad asociada
                    .Select(n => n.Actividad)   // Selecciona solo la actividad
                    .Distinct()                 // Evita duplicados en caso de múltiples notas para la misma actividad
                    .ToListAsync();

                return actividadesAlumno;
            }

            // Para administradores y profesores, devuelve todas las actividades
            return await _context.Actividades.ToListAsync();
        }

        // GET: /Actividades/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador,Profesor,Alumno")]
        public async Task<ActionResult<ActividadModel>> GetActividadPorId(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound("Actividad no encontrada.");
            }

            return actividad;
        }

        // POST: /Actividades/Registrar
        [HttpPost]
        [Route("Registrar")]
        [Authorize(Roles = "Administrador,Profesor")]
        public async Task<ActionResult<ActividadModel>> PostActividad(ActividadModel actividad)
        {
            _context.Actividades.Add(actividad);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActividadPorId), new { id = actividad.actividad_id }, actividad);
        }

        // PUT: /Actividades/Editar/{id}
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

        // DELETE: /Actividades/Eliminar/{id}
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
