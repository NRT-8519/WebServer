using Microsoft.EntityFrameworkCore;
using WebServer.Models.ClinicData;
using WebServer.Models.MedicineData;

namespace WebServer.Services.Contexts
{
    public partial class ScheduleContext : DbContext
    {
        public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options) { }

        public virtual DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(s => s.ScheduleId);
                entity.HasOne(s => s.Doctor).WithOne().HasForeignKey<Schedule>(s => s.DoctorUUID).IsRequired();
                entity.HasOne(s => s.Patient).WithOne().HasForeignKey<Schedule>(s => s.PatientUUID).IsRequired();
                entity.Property(s => s.ScheduledDateTime).IsRequired();
                entity.Property(s => s.Event).IsRequired();
            });
        }
    }
}
