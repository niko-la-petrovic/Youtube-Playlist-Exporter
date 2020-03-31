using Newtonsoft.Json;
using System;

namespace YoutubePlaylistExporter.Youtube
{
    public class Item
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public Snippet Snippet { get; set; }

        public PlaylistItem ToPlaylistItem()
        {
            return new PlaylistItem
            {
                DateTimeAdded = DateTimeOffset.MinValue,
                Etag = Etag,
                Id = Id,
                Kind = Kind,
                Snippet = Snippet
            };
        }
    }
}
