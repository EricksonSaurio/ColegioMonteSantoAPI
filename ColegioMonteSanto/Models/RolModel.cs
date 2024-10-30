using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("rol")]
    public class RolModel
    {
        [Key]
        public int rol_id { get; set; }

        public string nombre_rol { get; set; }
    }
}
