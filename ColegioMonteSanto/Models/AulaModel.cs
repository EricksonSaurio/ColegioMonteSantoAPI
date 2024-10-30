using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("aulas")]
    public class AulaModel
    {
        [Key]
        public int aula_id { get; set; }

        public string nombre_aula { get; set; }

        public int estado { get; set; } = 1;
    }
}
