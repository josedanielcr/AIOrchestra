using Microsoft.EntityFrameworkCore;
using PlaylistService.Common.Entities;
using PlaylistService.Database;

namespace PlaylistService.Shared
{
    public class PlaylistDbUtils
    {
        private readonly PlaylistManagementDbContext dbContext;

        public PlaylistDbUtils(PlaylistManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddPlaylistAsync(Playlist playlist)
        {
            await dbContext.Playlists.AddAsync(playlist);
            await dbContext.SaveChangesAsync();
        }

        public async Task<(Playlist, bool)> GetPlaylistAsync(string playlistId)
        {
            var result = await dbContext.Playlists.Where(p => p.Id == playlistId).FirstOrDefaultAsync();
            return (result!, result != null);
        }

        public async Task<(Playlist, bool)> GetPlaylistByNameAsync(string playlistName)
        {
            var result = await dbContext.Playlists.Where(p => p.Name == playlistName).FirstOrDefaultAsync();
            return (result!, result != null);
        }

        public async Task<Playlist> UpdatePlaylistAsync(Playlist playlist)
        {
            dbContext.Playlists.Update(playlist);
            await dbContext.SaveChangesAsync();
            return playlist;
        }

        public async Task<Playlist> AddSongToPlaylistAsync(string songId, string playlistId)
        {
            var playlist = await dbContext.Playlists.Where(p => p.Id == playlistId).FirstOrDefaultAsync()
                ?? throw new Exception("Playlist not found");
            playlist.SongIds.Add(songId);
            return await UpdatePlaylistAsync(playlist);
        }

        public async Task<List<Playlist>> GetUserPlaylistsAsync(string userId)
        {
            return await dbContext.Playlists.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task DeletePlaylistAsync(string playlistId)
        {
            var playlist = await dbContext.Playlists.Where(p => p.Id == playlistId).FirstOrDefaultAsync()
                ?? throw new Exception("Playlist not found");
            dbContext.Playlists.Remove(playlist);
            await dbContext.SaveChangesAsync();
        }
    }
}