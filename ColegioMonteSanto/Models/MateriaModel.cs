using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

       
        public int profesorid { get; set; } 

        
        [JsonIgnore]
        public ProfesorModel? Profesor { get; set; }
    }
}
