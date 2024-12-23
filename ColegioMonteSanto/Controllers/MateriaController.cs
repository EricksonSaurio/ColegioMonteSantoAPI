﻿using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColegioMonteSanto.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public MateriaController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador, Profesor, Estudiante")]
        
        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<MateriaModel>>> ListarMaterias()
        {
            return await _context.Materias.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaModel>> GetMateriaById(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound("Materia no encontrada");
            }
            return Ok(materia);
        }

       
        [Authorize(Roles = "Administrador")]
        [HttpPost("Registrar")]
        public async Task<ActionResult<MateriaModel>> RegistrarMateria([FromBody] MateriaModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Materias.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMateriaById), new { id = model.materia_id }, model);
        }

        
        [Authorize(Roles = "Administrador")]
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> EditarMateria(int id, [FromBody] MateriaModel model)
        {
            if (id != model.materia_id)
            {
                return BadRequest("ID de la materia no coincide");
            }

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound("Materia no encontrada");
            }

           
            materia.nombre_materia = model.nombre_materia;
            materia.estado = model.estado;
            materia.profesorid = model.profesorid;

            await _context.SaveChangesAsync();
            return NoContent(); 
        }
        
        [Authorize(Roles = "Administrador")]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarMateria(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound("Materia no encontrada");
            }

            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();
            return NoContent(); 
        }
    }
}
