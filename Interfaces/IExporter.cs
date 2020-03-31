using Newtonsoft.Json;

namespace YoutubePlaylistExporter.Interfaces
{
    public interface IExporter
    {
        /// <summary>
        /// Used to infer type of exporter interface: "playlist" or "item"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("folderName")]
        public string FolderName { get; set; }
        [JsonProperty("overwriteExisting")]
        public bool OverwriteExisting { get; set; }
        [JsonProperty("sortDescending")]
        public bool SortDescending { get; set; }
    }
}
