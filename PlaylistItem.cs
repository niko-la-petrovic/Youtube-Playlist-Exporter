using Newtonsoft.Json;
using System;
using YoutubePlaylistExporter.Youtube;

namespace YoutubePlaylistExporter
{
    public class PlaylistItem : Item
    {
        [JsonProperty("dateTimeAdded")]
        public DateTimeOffset DateTimeAdded { get; set; }
    }
}
