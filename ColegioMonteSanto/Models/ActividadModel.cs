using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("actividad")]
    public class ActividadModel
    {
        [Key]
        public int actividad_id { get; set; }

        public string nombre_actividad { get; set; }

        public int estado { get; set; }
    }
}
