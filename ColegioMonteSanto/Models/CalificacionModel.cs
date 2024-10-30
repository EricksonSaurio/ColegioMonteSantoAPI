using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("calificaciones")]
    public class CalificacionModel
    {
        [Key]
        public int calificacion_id { get; set; }

        [Required]
        public int alumno_id { get; set; }

        [Required]
        public int materia_id { get; set; }

        [Required]
        [Range(0, 100)] // Puedes ajustar el rango de la calificación
        public int nota { get; set; }

        // Propiedades de navegación para relaciones
        public AlumnoModel Alumno { get; set; }
        public MateriaModel Materia { get; set; }
    }
}
