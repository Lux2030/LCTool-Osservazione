using LCTool.Osservazione.Models;
using Microsoft.EntityFrameworkCore;

namespace LCTool.Osservazione.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Models.Osservazione>    Osservazioni           { get; set; }
        public DbSet<OsservazioniRilevataDa> OsservazioniRilevataDa { get; set; }
        public DbSet<OsservazioniStati>      OsservazioniStati      { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Osservazione>(entity =>
            {
                entity.ToTable("Osservazioni", "dbo");

                entity.Property(e => e.CreatoDa).HasMaxLength(256);
                entity.Property(e => e.ModificatoDa).HasMaxLength(256);
                entity.Property(e => e.RevisionatoDa).HasMaxLength(256);
            });
        }
    }
}
