using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("periodos")]
    public class PeriodoModel
    {
        [Key]
        public int periodo_id { get; set; }

        public string nombre_periodo { get; set; }

        public int estado { get; set; } = 1;
    }
}
