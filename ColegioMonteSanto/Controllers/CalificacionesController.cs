﻿using ColegioMonteSanto.Data;
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
    public class CalificacionesController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public CalificacionesController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        // GET: /Calificaciones/Listar
        // Permite que el Administrador y el Profesor puedan listar todas las calificaciones
        [HttpGet]
        [Route("Listar")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<IEnumerable<CalificacionModel>>> GetCalificaciones()
        {
            return await _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.Materia)
                .ToListAsync();
        }

        // GET: /Calificaciones/ListarPropias
        // Permite que el Alumno liste solo sus propias calificaciones
        [HttpGet]
        [Route("ListarPropias")]
        [Authorize(Roles = "Alumno")]
        public async Task<ActionResult<IEnumerable<CalificacionModel>>> GetCalificacionesPropias()
        {
            var alumnoIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(alumnoIdString))
            {
                return Unauthorized("No se pudo obtener el ID del alumno.");
            }

            var alumnoId = int.Parse(alumnoIdString);
            return await _context.Calificaciones
                .Where(c => c.alumno_id == alumnoId)
                .Include(c => c.Materia)
                .ToListAsync();
        }

        // GET: /Calificaciones/{id}
        // Permite que tanto el Administrador, el Profesor y el Alumno (propietario) vean una calificación específica
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador, Profesor, Alumno")]
        public async Task<ActionResult<CalificacionModel>> GetCalificacionPorId(int id)
        {
            var calificacion = await _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.Materia)
                .FirstOrDefaultAsync(c => c.calificacion_id == id);

            if (calificacion == null)
            {
                return NotFound("Calificación no encontrada.");
            }

            // Verificar si el usuario es un alumno y si la calificación le pertenece
            if (User.IsInRole("Alumno"))
            {
                var alumnoIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(alumnoIdString))
                {
                    return Unauthorized("No se pudo obtener el ID del alumno.");
                }

                var alumnoId = int.Parse(alumnoIdString);
                if (calificacion.alumno_id != alumnoId)
                {
                    return Forbid("No tienes acceso a esta calificación.");
                }
            }

            return calificacion;
        }

        // POST: /Calificaciones/Registrar
        // Solo el Administrador y el Profesor pueden registrar nuevas calificaciones
        [HttpPost]
        [Route("Registrar")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<ActionResult<CalificacionModel>> PostCalificacion(CalificacionModel calificacion)
        {
            _context.Calificaciones.Add(calificacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCalificacionPorId", new { id = calificacion.calificacion_id }, calificacion);
        }

        // PUT: /Calificaciones/Editar/{id}
        // Solo el Administrador y el Profesor pueden editar calificaciones
        [HttpPut]
        [Route("Editar/{id}")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<IActionResult> EditarCalificacion(int id, [FromBody] CalificacionModel calificacion)
        {
            if (id != calificacion.calificacion_id)
            {
                return BadRequest("El ID de la calificación no coincide.");
            }

            _context.Entry(calificacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalificacionExists(id))
                {
                    return NotFound("Calificación no encontrada.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: /Calificaciones/Eliminar/{id}
        // Solo el Administrador y el Profesor pueden eliminar calificaciones
        [HttpDelete]
        [Route("Eliminar/{id}")]
        [Authorize(Roles = "Administrador, Profesor")]
        public async Task<IActionResult> EliminarCalificacion(int id)
        {
            var calificacion = await _context.Calificaciones.FindAsync(id);

            if (calificacion == null)
            {
                return NotFound("Calificación no encontrada.");
            }

            _context.Calificaciones.Remove(calificacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar si una calificación existe
        private bool CalificacionExists(int id)
        {
            return _context.Calificaciones.Any(e => e.calificacion_id == id);
        }
    }
}
