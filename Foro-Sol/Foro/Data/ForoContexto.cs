using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foro
{
    public class ForoContexto : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ForoContexto(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MiembrosHabilitados>().HasKey(mh => new { mh.MiembroId, mh.EntradaId });
            modelBuilder.Entity<MiembrosHabilitados>()
      .HasKey(mh => new { mh.MiembroId, mh.EntradaId });

            modelBuilder.Entity<MiembrosHabilitados>()
                .HasOne(mh => mh.Miembro)
                .WithMany(m => m.MiembrosHabilitados)
                .HasForeignKey(mh => mh.MiembroId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MiembrosHabilitados>()
                .HasOne(mh => mh.Entrada)
                .WithMany(e => e.MiembrosHabilitados)
                .HasForeignKey(mh => mh.EntradaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaccion>().HasKey(r => new { r.MiembroId, r.RespuestaId });
            modelBuilder.Entity<Reaccion>().HasOne(r => r.Miembro)
                .WithMany(m => m.PreguntasYRespuestasQueMeGustan)
                .HasForeignKey(pyr => pyr.MiembroId);

            modelBuilder.Entity<Reaccion>().HasOne(r => r.Respuesta)
                .WithMany(res => res.Reacciones)
                .HasForeignKey(r => r.RespuestaId);

            #region Establecer nombres para el Identity Store
            modelBuilder.Entity<IdentityUser<int>>().ToTable("Usuarios");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UsuariosRoles");
            #endregion

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<MiembrosHabilitados> MiembrosHabilitados { get; set; }
        public DbSet<Reaccion> Reacciones { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Rol> Roles { get; set; }

    }
}
