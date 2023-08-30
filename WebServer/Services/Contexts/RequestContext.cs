using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.Models.MedicineData;

namespace WebServer.Services.Contexts
{
    public partial class RequestContext : DbContext
    {
        public RequestContext(DbContextOptions<RequestContext> options) : base(options) { }

        public virtual DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(r => r.UUID);
                entity.HasOne(r => r.Patient).WithOne().HasForeignKey<Request>(r => r.PatientUUID).IsRequired();
                entity.HasOne(r => r.Doctor).WithOne().HasForeignKey<Request>(r => r.DoctorUUID).IsRequired();
                entity.Property(r => r.Title).IsRequired();
                entity.Property(r => r.Description).IsRequired();
                entity.Property(r => r.Type).IsRequired();
                entity.Property(r => r.Status).IsRequired();
                entity.Property(r => r.Reason).IsRequired();
            });
        }
    }
}
