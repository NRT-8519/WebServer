using Microsoft.EntityFrameworkCore;
using WebServer.Models;

namespace WebServer.Services.Contexts
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {
        }

        public virtual DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.UUID);
                entity.HasOne(r => r.User).WithOne().HasForeignKey<Report>(r => r.UserUUID).IsRequired();
                entity.Property(r => r.Title).IsRequired();
                entity.Property(r => r.Description).IsRequired();
                entity.Property(r => r.Date).IsRequired();
            });
        }
    }
}
