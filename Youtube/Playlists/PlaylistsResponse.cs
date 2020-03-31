using Newtonsoft.Json;
using System.Collections.Generic;

namespace YoutubePlaylistExporter.Youtube.Playlists
{
    public class PlaylistsResponse
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
}
