using Newtonsoft.Json;

namespace YoutubePlaylistExporter.Youtube.Playlists
{
    public class Localized
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
