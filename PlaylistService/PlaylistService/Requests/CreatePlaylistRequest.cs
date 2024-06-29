namespace PlaylistService.Requests
{
    public class CreatePlaylistRequest
    {
        public string Name { get; set; } = String.Empty;
        public string UserId { get; set; } = String.Empty;
        public List<string> SongIds { get; set; } = new List<string>();
    }
}
