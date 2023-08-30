using Microsoft.EntityFrameworkCore;
using WebServer.Models.ClinicData;

namespace WebServer.Services.Contexts
{
    public class PrescriptionContext : DbContext
    {
        public PrescriptionContext(DbContextOptions<PrescriptionContext> options) : base(options) { }

        public virtual DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.DatePrescribed);
                entity.Property(p => p.DateAdministered);
                entity.Property(p => p.PrescriptionNotes);
                entity.HasOne(p => p.Doctor).WithOne().HasForeignKey<Prescription>(p => p.DoctorUUID).IsRequired();
                entity.HasOne(p => p.Patient).WithOne().HasForeignKey<Prescription>(p => p.PatientUUID).IsRequired();
                entity.HasOne(p => p.Medicine).WithOne().HasForeignKey<Prescription>(p => p.MedicineUUID).IsRequired();
            });
        }
    }
}
