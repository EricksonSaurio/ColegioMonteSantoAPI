using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("materias")]
    public class MateriaModel
    {
        [Key]
        public int materia_id { get; set; }

        [Required]
        public string nombre_materia { get; set; }

        [Required]
        public int estado { get; set; }

        // Relación con el Profesor
        public int ProfesorId { get; set; } // Agrega esta propiedad

        // Propiedad de navegación
        public ProfesorModel Profesor { get; set; }
    }
}
