using Microsoft.EntityFrameworkCore;
using WebServer.Models.MedicineData;

namespace WebServer.Services.Contexts
{
    public class MedicineContext : DbContext
    {
        public MedicineContext(DbContextOptions<MedicineContext> options) : base(options) { }

        public virtual DbSet<Medicine> Medicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.UUID).IsRequired();
                entity.Property(m => m.Name).IsRequired();
                entity.Property(m => m.Type).IsRequired();
                entity.Property(m => m.Dosage).IsRequired();
                entity.Property(m => m.DosageType).IsRequired();
                entity.Property(m => m.EAN).IsRequired();
                entity.Property(m => m.ATC).IsRequired();
                entity.Property(m => m.UniqueClassification);
                entity.Property(m => m.INN).IsRequired();
                entity.Property(m => m.PrescriptionType).IsRequired();
                entity.HasOne(m => m.Company).WithOne().HasForeignKey<Company>(c => c.Id).IsRequired();
                entity.HasOne(m => m.Issuer).WithOne().HasForeignKey<Issuer>(i => i.Id).IsRequired();
                entity.HasMany(m => m.Clearances).WithOne().HasForeignKey(c => c.MedicineId).IsRequired();
            });
        }
    }
}
