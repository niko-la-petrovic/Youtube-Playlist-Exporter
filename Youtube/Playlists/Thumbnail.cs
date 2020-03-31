using Newtonsoft.Json;
using System;

namespace YoutubePlaylistExporter.Youtube.Playlists
{
    public class Thumbnail
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }
}
