using ColegioMonteSanto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ColegioMonteSanto.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColegioMonteSanto.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public AlumnosController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<IEnumerable<AlumnoModel>>> GetAlumnos()
        {
            return await _context.Alumnos.ToListAsync();
        }

        [HttpGet("{carnet}")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<AlumnoModel>> GetAlumnoPorCarnet(string carnet)
        {
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.carnet == carnet);

            if (alumno == null)
            {
                return NotFound();
            }

            return alumno;
        }

        [HttpPost]
        [Route("Registrar")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<AlumnoModel>> PostAlumno(AlumnoModel alumno)
        {
            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlumnoPorCarnet", new { carnet = alumno.carnet }, alumno);
        }

        [HttpPut]
        [Route("Editar/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditarAlumno(int id, [FromBody] AlumnoModel alumno)
        {
            if (id != alumno.alumno_id)
            {
                return BadRequest("El ID del alumno no coincide.");
            }

            if (alumno == null)
            {
                return BadRequest("Se requiere un cuerpo de solicitud no vacío.");
            }

            _context.Entry(alumno).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(id))
                {
                    return NotFound("Alumno no encontrado.");
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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarAlumno(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);

            if (alumno == null)
            {
                return NotFound("Alumno no encontrado.");
            }

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlumnoExists(int id)
        {
            return _context.Alumnos.Any(e => e.alumno_id == id);
        }
    }
}
