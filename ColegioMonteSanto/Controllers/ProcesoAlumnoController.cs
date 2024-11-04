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
    [Authorize(Roles = "Alumno")]
    public class ProcesoAlumnoController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public ProcesoAlumnoController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        [HttpGet("MisCalificaciones")]
        public async Task<ActionResult<IEnumerable<CalificacionModel>>> MisCalificaciones()
        {
            var alumnoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var calificaciones = await _context.Calificaciones
                .Where(c => c.alumno_id == alumnoId)
                .Include(c => c.Materia)
                .ToListAsync();

            return Ok(calificaciones);
        }

        [HttpGet("ProgresoMateria/{materiaId}")]
        public async Task<ActionResult<CalificacionModel>> ProgresoMateria(int materiaId)
        {
            var alumnoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var calificaciones = await _context.Calificaciones
                .Where(c => c.alumno_id == alumnoId && c.materia_id == materiaId)
                .ToListAsync();

            return Ok(calificaciones);
        }

        [HttpGet("MateriasInscritas")]
        public async Task<ActionResult<IEnumerable<MateriaModel>>> MateriasInscritas()
        {
            var alumnoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var materias = await _context.Materias
                .Where(m => _context.Calificaciones.Any(c => c.alumno_id == alumnoId && c.materia_id == m.materia_id))
                .ToListAsync();

            return Ok(materias);
        }
    }
}
