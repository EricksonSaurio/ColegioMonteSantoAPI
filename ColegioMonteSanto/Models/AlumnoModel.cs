using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("alumnos")]
    public class AlumnoModel
    {
        [Key]
        public int alumno_id { get; set; }
        public string nombre_alumno { get; set; } = string.Empty;
        public string carnet { get; set; }
        public int edad { get; set; }
        public string direccion { get; set; }
        public string cedula { get; set; }
        public long telefono { get; set; }
        public string correo { get; set; }
        public DateTime fecha_nac { get; set; }
        public DateTime fecha_registro { get; set; }
        public int estado { get; set; }
    }
}
