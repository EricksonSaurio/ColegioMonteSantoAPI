using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColegioMonteSanto.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeriodosController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public PeriodosController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        // GET: /Periodos/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<PeriodoModel>>> GetPeriodos()
        {
            return await _context.Periodos.ToListAsync();
        }

        // GET: /Periodos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PeriodoModel>> GetPeriodoPorId(int id)
        {
            var periodo = await _context.Periodos.FindAsync(id);

            if (periodo == null)
            {
                return NotFound("Periodo no encontrado.");
            }

            return periodo;
        }

        // POST: /Periodos/Registrar
        [HttpPost]
        [Route("Registrar")]
        public async Task<ActionResult<PeriodoModel>> PostPeriodo(PeriodoModel periodo)
        {
            _context.Periodos.Add(periodo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPeriodoPorId), new { id = periodo.periodo_id }, periodo);
        }

        // PUT: /Periodos/Editar/5
        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> EditarPeriodo(int id, [FromBody] PeriodoModel periodo)
        {
            if (id != periodo.periodo_id)
            {
                return BadRequest("El ID del periodo no coincide.");
            }

            if (periodo == null)
            {
                return BadRequest("Se requiere un cuerpo de solicitud no vacío.");
            }

            _context.Entry(periodo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeriodoExists(id))
                {
                    return NotFound("Periodo no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: /Periodos/Eliminar/5
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> EliminarPeriodo(int id)
        {
            var periodo = await _context.Periodos.FindAsync(id);

            // Verificar si el periodo existe
            if (periodo == null)
            {
                return NotFound("Periodo no encontrado.");
            }

            // Remover el periodo de la base de datos
            _context.Periodos.Remove(periodo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar si un periodo existe
        private bool PeriodoExists(int id)
        {
            return _context.Periodos.Any(e => e.periodo_id == id);
        }
    }
}
