using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColegioMonteSanto.Models
{
    [Table("notas")]
    public class NotaModel
    {
        [Key]
        public int nota_id { get; set; }

        public int materia_id { get; set; }
        public int alumno_id { get; set; }
        public int actividad_id { get; set; }
        public int periodo_id { get; set; }
        public int valor_nota { get; set; }

        public DateTime fecha { get; set; }

        
        [ForeignKey("alumno_id")]
        public virtual AlumnoModel Alumno { get; set; }

        
        [ForeignKey("materia_id")]
        public virtual MateriaModel Materia { get; set; }

        
        [ForeignKey("actividad_id")]
        public virtual ActividadModel Actividad { get; set; }
    }
}
