using ApiUniRoom.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace ApiUniRoom
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FotoInfo>(entity =>
            {
                entity.HasNoKey(); 
            });
            modelBuilder.Entity<ReservaCompleta>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<HistorialPension>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<ReservaInfoCliente>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PagosAbonos>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<Pagos>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<PagoDetalle>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasNoKey();
            });
        }


        public DbSet<PensionPropietarioResponse> PensionPropietarioResponse { get; set; }
        public DbSet<Pensions> Pensiones { get; set; }
        public DbSet<FotoInfo> Foto { get; set; }
        public DbSet<ReservaCompleta> ReservaInfo { get; set; }
        public DbSet<HistorialPension> HistorialPensions { get; set; }
        public DbSet<ReservaInfoCliente> PensionC { get; set; }
        public DbSet<ApiUniRoom.Models.Perfil> Perfil { get; set; } = default!;
        public DbSet<ApiUniRoom.Models.Pagos> Pagos { get; set; } = default!;
        public DbSet<PagosAbonos> Abonos { get; set; }
        public DbSet<PagoDetalle> PagosPension { get; set; }
        public DbSet<ComentarioView> VistaComentarios { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }



    }
}
