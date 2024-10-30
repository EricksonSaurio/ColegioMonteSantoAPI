using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace ColegioMonteSanto.Controllers
{
    [Authorize(Roles="Administrador")]
    [Route("[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ColegioMonteSantoContext _context;

        public UsuariosController(ColegioMonteSantoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<UsuarioModel>>> GetUsuarios()
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<ActionResult<UsuarioModel>> PostUsuario(UsuarioModel usuario)
        {
            // Verificar si el rol especificado existe
            var rolExistente = await _context.Roles.FindAsync(usuario.rol_id);
            if (rolExistente == null)
            {
                return BadRequest("El rol especificado no existe.");
            }

            // Evita duplicar el rol en el objeto usuario
            usuario.Rol = null; // Asegura que no se intenta insertar un nuevo objeto Rol

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarios", new { id = usuario.usuario_id }, usuario);
        }

        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] UsuarioModel usuario)
        {
            if (id != usuario.usuario_id)
            {
                return BadRequest("El ID del usuario no coincide.");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound("Usuario no encontrado.");
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
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.usuario_id == id);
        }
    }
}