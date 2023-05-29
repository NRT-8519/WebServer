using Microsoft.EntityFrameworkCore;
using WebServer.Models.MedicineData;

namespace WebServer.Services.Contexts
{
    public partial class ClearanceContext : DbContext
    {
        public ClearanceContext(DbContextOptions<ClearanceContext> options) : base(options) { }

        public virtual DbSet<Clearance> Clearances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Clearance>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.UUID).IsRequired();
                entity.Property(c => c.ClearanceNumber).IsRequired();
                entity.Property(c => c.BeginDate).IsRequired();
                entity.Property(c => c.ExpiryDate).IsRequired();
                entity.HasOne(c => c.Medicine).WithOne(m => m.Clearance).HasForeignKey((Medicine m) => m.ClearanceId).IsRequired();
            });
        }
    }
}
