using Microsoft.EntityFrameworkCore;
using Foro;

namespace Foro
{
    public class ForoContexto : DbContext
    {
        public ForoContexto(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MiembrosHabilitados>().HasKey(mh => new { mh.MiembroId, mh.EntradaId });
            modelBuilder.Entity<MiembrosHabilitados>().HasOne(mh => mh.Miembro)
                .WithMany(m => m.miembrosHabilitados)
                .HasForeignKey(mh => mh.MiembroId);


            modelBuilder.Entity<MiembrosHabilitados>().HasOne(mh => mh.Entrada)
                .WithMany(e => e.MiembrosHabilitados)
                .HasForeignKey(mh => mh.EntradaId);


            modelBuilder.Entity<Reaccion>().HasKey(r => new { r.MiembroId,r.RespuestaId });
            modelBuilder.Entity<Reaccion>().HasOne(r => r.Miembro)
                .WithMany(m => m.PreguntasYRespuestasQueMeGustan)
                .HasForeignKey(pyr => pyr.MiembroId);


            modelBuilder.Entity<Reaccion>().HasOne(r => r.Respuesta)
                .WithMany(res => res.Reacciones)
                .HasForeignKey(r => r.RespuestaId);

        }


        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Foro.MiembrosHabilitados>? MiembrosHabilitados { get; set; }
        public DbSet<Foro.Reaccion>? Reaccion { get; set; }
        public DbSet<Foro.Pregunta>? Pregunta { get; set; }





    }
}
