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

        // Relaci�n con AlumnoModel
        [ForeignKey("alumno_id")]
        public virtual AlumnoModel Alumno { get; set; }

        // Relaci�n con MateriaModel
        [ForeignKey("materia_id")]
        public virtual MateriaModel Materia { get; set; }

        // Relaci�n con ActividadModel
        [ForeignKey("actividad_id")]
        public virtual ActividadModel Actividad { get; set; }
    }
}
