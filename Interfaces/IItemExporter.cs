using Newtonsoft.Json;
using System.Collections.Generic;

namespace YoutubePlaylistExporter.Interfaces
{
    public interface IItemExporter : IExporter
    {
        [JsonProperty("individualItemFormat")]
        public string IndividualItemFormat { get; set; }
        [JsonProperty("defaultImplementation")]
        public bool DefaultImplementation { get; set; }

        [JsonProperty("dateTimeFormat")]
        public string DateTimeFormat { get; set; }

        public void Export(List<PlaylistItem> playlistItems, string playlistName);
    }
}
