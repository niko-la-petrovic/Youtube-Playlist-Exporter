using System.Collections.Generic;

namespace YoutubePlaylistExporter
{
    public class Playlist
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public long TotalItems { get; set; }
        public List<PlaylistItem> PlaylistItems { get; set; }
        public Playlist()
        {
            Id = null;
            TotalItems = 0;
            PlaylistItems = new List<PlaylistItem>();
        }

    }

}
