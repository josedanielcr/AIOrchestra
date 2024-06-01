using AIOrchestra.UserManagementService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIOrchestra.UserManagementService.Database
{
    public class UserManagementDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);
        }
    }
}
