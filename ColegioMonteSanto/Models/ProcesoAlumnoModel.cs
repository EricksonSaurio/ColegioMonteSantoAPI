using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("procesos_alumnos")]
    public class ProcesoAlumnoModel
    {
        [Key]
        public int procesoa_id { get; set; }

        [ForeignKey("Alumno")]
        public int alumno_id { get; set; }

        [ForeignKey("Proceso")]
        public int proceso_id { get; set; }

        [Required]
        public int estado { get; set; }

        // Relaciones para las llaves foráneas
        public virtual AlumnoModel Alumno { get; set; }
        public virtual ProcesoAlumnoModel proceso { get; set; }
    }
}
