using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ColegioMonteSanto.Controllers
{
    [Authorize(Roles = "Administrador")]
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
                .Include(u => u.Profesor)
                .Include(u => u.Alumno)
                .ToListAsync();
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<ActionResult<UsuarioModel>> PostUsuario(UsuarioModel usuario)
        {
            var rolExistente = await _context.Roles.FindAsync(usuario.rol_id);
            if (rolExistente == null)
            {
                return BadRequest("El rol especificado no existe.");
            }

            if (usuario.rol_id == 2 && usuario.profesor_id != null)
            {
                var profesor = await _context.Profesores.FindAsync(usuario.profesor_id);
                if (profesor == null)
                {
                    return BadRequest("El profesor especificado no existe.");
                }
                usuario.nombre = profesor.nombre;
                usuario.alumno_id = null;
            }
            else if (usuario.rol_id == 3 && usuario.alumno_id != null)
            {
                var alumno = await _context.Alumnos.FindAsync(usuario.alumno_id);
                if (alumno == null)
                {
                    return BadRequest("El alumno especificado no existe.");
                }
                usuario.nombre = alumno.nombre_alumno;
                usuario.profesor_id = null;
            }
            else
            {
                if (usuario.rol_id == 2)
                {
                    return BadRequest("Para el rol de Profesor, debe especificar un profesor_id válido.");
                }
                else if (usuario.rol_id == 3)
                {
                    return BadRequest("Para el rol de Estudiante, debe especificar un alumno_id válido.");
                }
                else
                {
                    return BadRequest("Debe especificar un rol válido y los correspondientes profesor_id o alumno_id.");
                }
            }

            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.usuario == usuario.usuario);
            if (usuarioExistente != null)
            {
                return BadRequest("El nombre de usuario ya está en uso.");
            }

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

            var rolExistente = await _context.Roles.FindAsync(usuario.rol_id);
            if (rolExistente == null)
            {
                return BadRequest("El rol especificado no existe.");
            }

            if (usuario.rol_id == 2 && usuario.profesor_id != null)
            {
                var profesor = await _context.Profesores.FindAsync(usuario.profesor_id);
                if (profesor == null)
                {
                    return BadRequest("El profesor especificado no existe.");
                }
                usuario.nombre = profesor.nombre;
                usuario.alumno_id = null;
            }
            else if (usuario.rol_id == 3 && usuario.alumno_id != null)
            {
                var alumno = await _context.Alumnos.FindAsync(usuario.alumno_id);
                if (alumno == null)
                {
                    return BadRequest("El alumno especificado no existe.");
                }
                usuario.nombre = alumno.nombre_alumno;
                usuario.profesor_id = null;
            }
            else
            {
                if (usuario.rol_id == 2)
                {
                    return BadRequest("Para el rol de Profesor, debe especificar un profesor_id válido.");
                }
                else if (usuario.rol_id == 3)
                {
                    return BadRequest("Para el rol de Estudiante, debe especificar un alumno_id válido.");
                }
                else
                {
                    return BadRequest("Debe especificar un rol válido y los correspondientes profesor_id o alumno_id.");
                }
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
