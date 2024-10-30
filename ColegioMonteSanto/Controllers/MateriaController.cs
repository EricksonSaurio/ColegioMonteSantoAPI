using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ColegioMonteSanto.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private readonly IMateriaService _materiaService;

        public MateriaController(IMateriaService materiaService)
        {
            _materiaService = materiaService;
        }

        // Obtener materias asignadas al profesor autenticado
        [HttpGet("profesor")]
        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> GetMateriasProfesor()
        {
            var profesorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var materias = await _materiaService.GetMateriasByProfesorIdAsync(int.Parse(profesorId));
            return Ok(materias);
        }

        // Obtener materias asignadas al alumno autenticado
        [HttpGet("alumno")]
        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> GetMateriasAlumno()
        {
            var alumnoId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var materias = await _materiaService.GetMateriasByAlumnoIdAsync(int.Parse(alumnoId));
            return Ok(materias);
        }

        // Crear, actualizar y eliminar materias (solo para administrador)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateMateria([FromBody] MateriaModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _materiaService.CreateMateriaAsync(model);
            return CreatedAtAction(nameof(GetMateriaById), new { id = result.materia_id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateMateria(int id, [FromBody] MateriaModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _materiaService.UpdateMateriaAsync(id, model);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteMateria(int id)
        {
            var result = await _materiaService.DeleteMateriaAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Profesor, Alumno, Administrador")]
        public async Task<IActionResult> GetMateriaById(int id)
        {
            var materia = await _materiaService.GetMateriaByIdAsync(id);
            return materia != null ? Ok(materia) : NotFound();
        }
    }

}
