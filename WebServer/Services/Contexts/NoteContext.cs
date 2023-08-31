using Microsoft.EntityFrameworkCore;
using WebServer.Models.ClinicData;

namespace WebServer.Services.Contexts
{
    public class NoteContext : DbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {
        }

        public virtual DbSet<Notes> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.HasOne(n => n.Doctor).WithOne().HasForeignKey<Notes>(n => n.DoctorUUID).IsRequired();
                entity.HasOne(n => n.Patient).WithOne().HasForeignKey<Notes>(n => n.PatientUUID).IsRequired();
                entity.Property(n => n.NoteTitle).IsRequired();
                entity.Property(n => n.Note).IsRequired();
                entity.Property(n => n.NoteDate).IsRequired();
            });
        }
    }
}
