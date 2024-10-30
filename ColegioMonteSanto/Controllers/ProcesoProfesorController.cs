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

        // GET: /ProcesoProfesor/ListarAlumnosPorMateria/{materiaId}
        // Permite que el profesor obtenga la lista de alumnos en una materia específica
        [HttpGet("ListarAlumnosPorMateria/{materiaId}")]
        public async Task<ActionResult<IEnumerable<AlumnoModel>>> ListarAlumnosPorMateria(int materiaId)
        {
            var alumnos = await _context.Alumnos
                .Where(a => _context.Calificaciones.Any(c => c.alumno_id == a.alumno_id && c.materia_id == materiaId))
                .ToListAsync();

            return Ok(alumnos);
        }

        // POST: /ProcesoProfesor/AsignarCalificacion
        // Permite al profesor asignar una calificación a un alumno
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

        // GET: /ProcesoProfesor/MateriasAsignadas
        // Permite que el profesor vea las materias que tiene asignadas
        [HttpGet("MateriasAsignadas")]
        public async Task<ActionResult<IEnumerable<MateriaModel>>> MateriasAsignadas()
        {
            var profesorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var materias = await _context.Materias
                .Where(m => m.ProfesorId == profesorId) // Asegúrate de que exista ProfesorId en MateriaModel
                .ToListAsync();

            return Ok(materias);
        }
    }
}
