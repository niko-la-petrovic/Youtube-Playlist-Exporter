using System.Collections.Generic;

namespace YoutubePlaylistExporter
{
    public class Utility
    {
        public static void SortListPlaylistItems(List<PlaylistItem> playlistItems, bool sortDescending)
        {
            playlistItems.Sort((x, y) => x.Snippet.PublishedAt.CompareTo(y.Snippet.PublishedAt));
            if (sortDescending)
            {
                playlistItems.Reverse();
            }
        }
    }
}
