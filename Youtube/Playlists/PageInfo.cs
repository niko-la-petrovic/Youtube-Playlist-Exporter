using Newtonsoft.Json;

namespace YoutubePlaylistExporter.Youtube.Playlists
{
    public class PageInfo
    {
        [JsonProperty("totalResults")]
        public long TotalResults { get; set; }

        [JsonProperty("resultsPerPage")]
        public long ResultsPerPage { get; set; }
    }
}
