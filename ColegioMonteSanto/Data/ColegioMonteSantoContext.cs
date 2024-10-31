using Microsoft.EntityFrameworkCore;
using ColegioMonteSanto.Models;

namespace ColegioMonteSanto.Data
{
    public class ColegioMonteSantoContext : DbContext
    {
        public ColegioMonteSantoContext(DbContextOptions<ColegioMonteSantoContext> options) : base(options) { }

        public DbSet<AlumnoModel> Alumnos { get; set; }
        public DbSet<MateriaModel> Materias { get; set; }
        public DbSet<NotaModel> Notas { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<ActividadModel> Actividades { get; set; }
        public DbSet<AulaModel> Aulas { get; set; }
        public DbSet<CalificacionModel> Calificaciones { get; set; }
        public DbSet<GradoModel> Grados { get; set; }
        public DbSet<PeriodoModel> Periodos { get; set; }
        public DbSet<ProcesoAlumnoModel> ProcesosAlumno { get; set; }
        public DbSet<ProcesoProfesorModel> ProcesoProfesor { get; set; }
        public DbSet<ProfesorModel> Profesores { get; set; }
        public DbSet<RolModel> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Definición explícita de la relación entre UsuarioModel y RolModel
            modelBuilder.Entity<UsuarioModel>()
                .HasOne(u => u.Rol)
                .WithMany() // Si RolModel no tiene una lista de usuarios, usamos WithMany()
                .HasForeignKey(u => u.rol_id)
                .OnDelete(DeleteBehavior.Restrict); // Define que el borrado de un rol no afecte a usuarios

            // Relación opcional con Profesor
            modelBuilder.Entity<UsuarioModel>()
                .HasOne(u => u.Profesor)
                .WithMany() // Asumiendo que no tienes una lista de usuarios en ProfesorModel
                .HasForeignKey(u => u.profesor_id)
                .OnDelete(DeleteBehavior.Restrict); // No elimina el usuario si se elimina el profesor

            // Relación opcional con Alumno
            modelBuilder.Entity<UsuarioModel>()
                .HasOne(u => u.Alumno)
                .WithMany() // Asumiendo que no tienes una lista de usuarios en AlumnoModel
                .HasForeignKey(u => u.alumno_id)
                .OnDelete(DeleteBehavior.Restrict); // No elimina el usuario si se elimina el alumno

            // Configura las relaciones en NotaModel (ya configuradas)
            modelBuilder.Entity<NotaModel>()
                .HasOne(n => n.Actividad)
                .WithMany()
                .HasForeignKey(n => n.actividad_id);

            modelBuilder.Entity<NotaModel>()
                .HasOne(n => n.Alumno)
                .WithMany()
                .HasForeignKey(n => n.alumno_id);

            modelBuilder.Entity<NotaModel>()
                .HasOne(n => n.Materia)
                .WithMany()
                .HasForeignKey(n => n.materia_id);
        }
    }
 }

