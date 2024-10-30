﻿using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace ColegioMonteSanto.Controllers
{
    [Authorize(Roles="Administrador")]
    [Route("[controller]")]
    [ApiController]
    public class ProfesoresController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public ProfesoresController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        // GET: /Profesores/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<ProfesorModel>>> GetProfesores()
        {
            return await _context.Profesores.ToListAsync();
        }

        // GET: /Profesores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfesorModel>> GetProfesorPorId(int id)
        {
            var profesor = await _context.Profesores.FindAsync(id);

            if (profesor == null)
            {
                return NotFound("Profesor no encontrado.");
            }

            return profesor;
        }

        // POST: /Profesores/Registrar
        [HttpPost]
        [Route("Registrar")]
        public async Task<ActionResult<ProfesorModel>> PostProfesor(ProfesorModel profesor)
        {
            _context.Profesores.Add(profesor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfesorPorId), new { id = profesor.profesor_id }, profesor);
        }

        // PUT: /Profesores/Editar/5
        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> EditarProfesor(int id, [FromBody] ProfesorModel profesor)
        {
            if (id != profesor.profesor_id)
            {
                return BadRequest("El ID del profesor no coincide.");
            }

            if (profesor == null)
            {
                return BadRequest("Se requiere un cuerpo de solicitud no vacío.");
            }

            _context.Entry(profesor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesorExists(id))
                {
                    return NotFound("Profesor no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: /Profesores/Eliminar/5
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> EliminarProfesor(int id)
        {
            var profesor = await _context.Profesores.FindAsync(id);

            // Verificar si el profesor existe
            if (profesor == null)
            {
                return NotFound("Profesor no encontrado.");
            }

            // Remover el profesor de la base de datos
            _context.Profesores.Remove(profesor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar si un profesor existe
        private bool ProfesorExists(int id)
        {
            return _context.Profesores.Any(e => e.profesor_id == id);
        }
    }
}
