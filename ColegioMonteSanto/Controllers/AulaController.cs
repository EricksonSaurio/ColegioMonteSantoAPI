﻿using ColegioMonteSanto.Data;
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
    public class AulasController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public AulasController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        // GET: /Aulas/Listar
        // Permite que tanto el Administrador como el Profesor puedan listar las aulas
        [HttpGet]
        [Route("Listar")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<IEnumerable<AulaModel>>> GetAulas()
        {
            return await _context.Aulas.ToListAsync();
        }

        // GET: /Aulas/{id}
        // Permite que tanto el Administrador como el Profesor puedan ver una aula específica
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<AulaModel>> GetAulaPorId(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);

            if (aula == null)
            {
                return NotFound("Aula no encontrada.");
            }

            return aula;
        }

        // POST: /Aulas/Registrar
        // Solo el Administrador puede registrar nuevas aulas
        [HttpPost]
        [Route("Registrar")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<AulaModel>> PostAula(AulaModel aula)
        {
            _context.Aulas.Add(aula);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAulaPorId), new { id = aula.aula_id }, aula);
        }

        // PUT: /Aulas/Editar/{id}
        // Solo el Administrador puede editar aulas
        [HttpPut]
        [Route("Editar/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditarAula(int id, [FromBody] AulaModel aula)
        {
            if (id != aula.aula_id)
            {
                return BadRequest("El ID del aula no coincide.");
            }

            if (aula == null)
            {
                return BadRequest("Se requiere un cuerpo de solicitud no vacío.");
            }

            _context.Entry(aula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AulaExists(id))
                {
                    return NotFound("Aula no encontrada.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: /Aulas/Eliminar/{id}
        // Solo el Administrador puede eliminar aulas
        [HttpDelete]
        [Route("Eliminar/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarAula(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);

            if (aula == null)
            {
                return NotFound("Aula no encontrada.");
            }

            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar si un aula existe
        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.aula_id == id);
        }
    }
}
