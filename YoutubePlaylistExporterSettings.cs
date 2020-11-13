using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YoutubePlaylistExporter.Interfaces;

namespace YoutubePlaylistExporter
{
    public class YoutubePlaylistExporterSettings
    {
        [JsonProperty("publicPlaylistIds")]
        public List<string> PublicPlaylistIds { get; set; }
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("sortDescending")]
        public bool SortDescending { get; set; }
        [JsonProperty("overwriteExisting")]
        public bool OverwriteExisting { get; set; }
        [JsonProperty("maximumAPICalls")]
        public int MaximumAPICalls { get; set; }

        public List<IItemExporter> ItemExporters { get; set; }

        public List<IPlaylistExporter> PlaylistExporters { get; set; }

        public YoutubePlaylistExporterSettings()
        {
            PublicPlaylistIds = new List<string>();
            ItemExporters = new List<IItemExporter>();
            PlaylistExporters = new List<IPlaylistExporter>();
        }

        /// <summary>
        /// Deserializes the YoutubeExporterSettings.json file to an instance of this class to bind the settings to its properties.
        /// </summary>
        /// <returns></returns>
        public static YoutubePlaylistExporterSettings SettingsFromFile()
        {
            try
            {
                string settingsJson = File.ReadAllText("YoutubeExporterSettings.json");
                var youtubePlaylistExporterSettings = JsonConvert.DeserializeObject<YoutubePlaylistExporterSettings>(settingsJson);

                try
                {
                    var exporters = JObject.Parse(settingsJson)["exporters"];
                    for (int i = 0; i < exporters.Count(); i++)
                    {
                        try
                        {
                            if (exporters[i]["type"].ToString() == "playlist" &&
                                exporters[i]["defaultImplementation"].ToObject<bool>())
                            {
                                youtubePlaylistExporterSettings.PlaylistExporters.Add(exporters[i].ToObject<DefaultPlaylistExporter>());
                            }
                            else if (exporters[i]["type"].ToString() == "item" &&
                                exporters[i]["defaultImplementation"].ToObject<bool>())
                            {
                                youtubePlaylistExporterSettings.ItemExporters.Add(exporters[i].ToObject<DefaultItemExporter>());
                            }
                        }
                        catch (NullReferenceException) { }
                    }
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("The exporters section is missing in the 'YoutubeExporterSettings.json' file. Continuing...");
                }

                return youtubePlaylistExporterSettings;
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Please check your settings in 'YoutubeExporterSettings.json'. A formatting error was encountered.");
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Could not successfully read 'YoutubeExporterSettings.json'. Check that the file exists.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new YoutubePlaylistExporterSettings();
        }

    }

}
