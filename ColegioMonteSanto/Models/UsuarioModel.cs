using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("usuarios")]
    public class UsuarioModel
    {
        [Key]
        public int usuario_id { get; set; }

        public string nombre { get; set; }

        public string usuario { get; set; }

        public string clave { get; set; }

        [ForeignKey("Rol")] // Relaciona explicitamente el rol_id con la propiedad Rol
        public int rol_id { get; set; }

        public int estado { get; set; } = 1;

        // La propiedad de navegación Rol, la hacemos opcional para evitar conflictos
        public virtual RolModel? Rol { get; set; }
    }
}
