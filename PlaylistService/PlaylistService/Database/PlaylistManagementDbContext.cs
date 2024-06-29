using Microsoft.EntityFrameworkCore;
using PlaylistService.Common.Entities;

namespace PlaylistService.Database
{
    public class PlaylistManagementDbContext : DbContext
    {
        public DbSet<Playlist> Playlists { get; set; }

        public PlaylistManagementDbContext(DbContextOptions<PlaylistManagementDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>().HasKey(p => p.Id);
        }
    }
}
