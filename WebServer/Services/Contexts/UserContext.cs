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
                entity.Property(u => u.Password);
                entity.Property(u => u.FirstName).IsRequired();
                entity.Property(u => u.MiddleName);
                entity.Property(u => u.LastName).IsRequired();
                entity.Property(u => u.Title);
                entity.Property(u => u.DateOfBirth).IsRequired();
                entity.Property(u => u.SSN).IsRequired();
                entity.Property(u => u.Gender).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PhoneNumber).IsRequired();
                entity.Property(u => u.IsDisabled).IsRequired();
                entity.Property(u => u.IsExpired).IsRequired();
                entity.Property(u => u.PasswordExpiryDate).IsRequired();
                entity.Property(u => u.Role).IsRequired();
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasMany(d => d.Notes).WithOne().HasForeignKey(n => n.DoctorUUID);
                entity.HasMany(d => d.Prescriptions).WithOne().HasForeignKey(p => p.DoctorUUID);
                entity.HasMany(d => d.Schedules).WithOne().HasForeignKey(s => s.DoctorUUID);
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
