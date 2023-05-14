using Microsoft.EntityFrameworkCore;
using WebServer.Models;

namespace WebServer.Services.Contexts
{
    public class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options) { }

        public virtual DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.UUID).IsRequired();
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.Country).IsRequired();
            });
        }
    }
}
