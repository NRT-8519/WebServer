using Microsoft.EntityFrameworkCore;
using WebServer.Models.MedicineData;

namespace WebServer.Services.Contexts
{
    public partial class IssuerContext : DbContext
    {
        public IssuerContext(DbContextOptions<IssuerContext> options) : base(options) { }

        public virtual DbSet<Issuer> Issuers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Issuer>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.UUID).IsRequired();
                entity.Property(i => i.Name).IsRequired();
                entity.Property(i => i.City).IsRequired();
                entity.Property(i => i.Area).IsRequired();
                entity.HasMany(i => i.Medicines).WithOne().HasForeignKey(m => m.IssuerId).IsRequired();
            });
        }
    }
}
