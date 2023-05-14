using Microsoft.EntityFrameworkCore;
using WebServer.Models.UserData;

namespace WebServer.Services.Contexts
{
    public partial class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UUID).IsRequired();
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.HasMany(u => u.Emails).WithOne().HasForeignKey(e => e.UserId).IsRequired();
                entity.HasMany(u => u.PhoneNumbers).WithOne().HasForeignKey(p => p.UserId).IsRequired();
                entity.Property(u => u.IsDisabled).IsRequired();
                entity.Property(u => u.IsExpired).IsRequired();
                entity.Property(u => u.PasswordExpiryDate).IsRequired();
                entity.HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired();
            });
        }
    }
}
