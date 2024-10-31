using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("usuarios")]
    public class UsuarioModel
    {
        [Key]
        public int usuario_id { get; set; }

        public string? nombre { get; set; }

        public string usuario { get; set; }

        public string clave { get; set; }

        public int rol_id { get; set; }

        public int estado { get; set; } = 1;

        public int? profesor_id { get; set; }
        public int? alumno_id { get; set; }

        public virtual RolModel? Rol { get; set; }
        public virtual ProfesorModel? Profesor { get; set; }
        public virtual AlumnoModel? Alumno { get; set; }
    }
}
