using Microsoft.EntityFrameworkCore;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.UserData;

namespace WebServer.Services.Contexts
{
    public partial class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UUID);
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.IsDisabled).IsRequired();
                entity.Property(u => u.IsExpired).IsRequired();
                entity.Property(u => u.PasswordExpiryDate).IsRequired();
                entity.HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserUUID).IsRequired();
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasMany(d => d.Notes).WithOne().HasForeignKey(n => n.DoctorUUID);
                entity.HasMany(d => d.Prescriptions).WithOne().HasForeignKey(p => p.DoctorUUID);
                entity.HasMany(d => d.Schedules).WithOne().HasForeignKey(s => s.DoctorUUID);
                entity.HasMany(d => d.TimeOffs).WithOne().HasForeignKey(t => t.DoctorUUID);
                entity.HasMany(d => d.WorkShifts).WithOne().HasForeignKey(w => w.DoctorUUID);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasOne(p => p.AssignedDoctor).WithOne().HasForeignKey<Patient>(p => p.DoctorUUID);
                entity.HasMany(d => d.Notes).WithOne().HasForeignKey(n => n.PatientUUID);
                entity.HasMany(d => d.Prescriptions).WithOne().HasForeignKey(p => p.PatientUUID);
                entity.HasMany(d => d.Schedules).WithOne().HasForeignKey(s => s.PatientUUID);
            });
        }
    }
}
