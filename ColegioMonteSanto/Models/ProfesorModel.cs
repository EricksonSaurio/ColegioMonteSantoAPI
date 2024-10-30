using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("profesor")]
    public class ProfesorModel
    {
        [Key]
        public int profesor_id { get; set; }

        public string nombre { get; set; }

        public string direccion { get; set; }

        public string cedula { get; set; }

        public long telefono { get; set; }

        public string correo { get; set; }

        public string nivel_est { get; set; }

        public int estado { get; set; } 
    }
}
