using Newtonsoft.Json;

namespace YoutubePlaylistExporter.Youtube
{
    public class ResourceId
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }
}
