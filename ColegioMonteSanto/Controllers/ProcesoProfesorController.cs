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
    [Authorize(Roles = "Profesor")]
    public class ProcesoProfesorController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public ProcesoProfesorController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        [HttpGet("ListarAlumnosPorMateria/{materiaId}")]
        public async Task<ActionResult<IEnumerable<AlumnoModel>>> ListarAlumnosPorMateria(int materiaId)
        {
            var alumnos = await _context.Alumnos
                .Where(a => _context.Calificaciones.Any(c => c.alumno_id == a.alumno_id && c.materia_id == materiaId))
                .ToListAsync();

            return Ok(alumnos);
        }

        [HttpPost("AsignarCalificacion")]
        public async Task<ActionResult> AsignarCalificacion(int alumnoId, int materiaId, int nota)
        {
            var calificacion = new CalificacionModel
            {
                alumno_id = alumnoId,
                materia_id = materiaId,
                nota = nota
            };

            _context.Calificaciones.Add(calificacion);
            await _context.SaveChangesAsync();

            return Ok("Calificación asignada correctamente.");
        }

        [HttpGet("MateriasAsignadas")]
        public async Task<ActionResult<IEnumerable<MateriaModel>>> MateriasAsignadas()
        {
            var profesorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var materias = await _context.Materias
                .Where(m => m.profesorid == profesorId)
                .ToListAsync();

            return Ok(materias);
        }
    }
}
