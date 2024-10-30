using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColegioMonteSanto.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public RolesController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        // GET: /Roles/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<RolModel>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // GET: /Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolModel>> GetRolPorId(int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }

            return rol;
        }

        // POST: /Roles/Registrar
        [HttpPost]
        [Route("Registrar")]
        public async Task<ActionResult<RolModel>> PostRol(RolModel rol)
        {
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRolPorId), new { id = rol.rol_id }, rol);
        }

        // PUT: /Roles/Editar/5
        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> EditarRol(int id, [FromBody] RolModel rol)
        {
            if (id != rol.rol_id)
            {
                return BadRequest("El ID del rol no coincide.");
            }

            if (rol == null)
            {
                return BadRequest("Se requiere un cuerpo de solicitud no vacío.");
            }

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
                {
                    return NotFound("Rol no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: /Roles/Eliminar/5
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> EliminarRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            // Verificar si el rol existe
            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }

            // Remover el rol de la base de datos
            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar si un rol existe
        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.rol_id == id);
        }
    }
}

