using Microsoft.EntityFrameworkCore;

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

        }


        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Reaccion> Reacciones { get; set; }





    }
}
