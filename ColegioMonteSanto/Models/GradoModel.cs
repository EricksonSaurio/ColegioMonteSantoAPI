using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("grados")]
    public class GradoModel
    {
        [Key]
        public int grado_id { get; set; }

        public string nombre_grado { get; set; }

        public int estado { get; set; } = 1;
    }
}
