using Newtonsoft.Json;

namespace YoutubePlaylistExporter.Youtube
{
    public class Thumbnails
    {
        [JsonProperty("default")]
        public Thumbnail Default { get; set; }

        [JsonProperty("medium")]
        public Thumbnail Medium { get; set; }

        [JsonProperty("high")]
        public Thumbnail High { get; set; }

        [JsonProperty("standard")]
        public Thumbnail Standard { get; set; }

        [JsonProperty("maxres")]
        public Thumbnail Maxres { get; set; }
    }
}
