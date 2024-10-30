using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("procesos")]
    public class ProcesoProfesorModel
    {
        [Key]
        public int proceso_id { get; set; }

        [ForeignKey("Grado")]
        public int grado_id { get; set; }

        [ForeignKey("Aula")]
        public int aula_id { get; set; }

        [ForeignKey("Profesor")]
        public int profesor_id { get; set; }

        public int estado { get; set; }

        // Relaciones para las llaves foráneas
        public virtual GradoModel grado { get; set; }
        public virtual AulaModel aula { get; set; }
        public virtual ProfesorModel profesor { get; set; }
    }
}
