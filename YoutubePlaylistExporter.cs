using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using YoutubePlaylistExporter.Interfaces;
using YoutubePlaylistExporter.Youtube;

namespace YoutubePlaylistExporter
{
    public class YoutubePlaylistExporter
    {

        public YoutubePlaylistExporterSettings YoutubePlaylistExporterSettings { get; set; }

        /// <summary>
        /// The default consturctor will load the settings from the YoutubeExporterSettings.json.
        /// </summary>
        public YoutubePlaylistExporter()
        {
            YoutubePlaylistExporterSettings = YoutubePlaylistExporterSettings.SettingsFromFile();
        }

        /// <summary>
        /// Passes each playlist first to the IPlaylistExporter interface's Export method and then to each IExporter interface's Export method.
        /// </summary>
        public void ExportPublicPlaylists()
        {
            List<Playlist> playlistsNamesAndIds = new List<Playlist>();
            try
            {
                playlistsNamesAndIds = GetPlaylistsWithNames(YoutubePlaylistExporterSettings.PublicPlaylistIds);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            foreach (string playlistId in YoutubePlaylistExporterSettings.PublicPlaylistIds)
            {
                try
                {
                    Playlist currentPlaylist = GetPlaylistAllItems(playlistId);
                    try
                    {
                        currentPlaylist.Name = playlistsNamesAndIds.First(p => p.Id == currentPlaylist.Id).Name;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                    foreach (IPlaylistExporter playlistExporter in YoutubePlaylistExporterSettings.PlaylistExporters)
                    {
                        try
                        {
                            currentPlaylist = playlistExporter.Export(currentPlaylist);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Encountered exception with an item exporter.");
                            Console.WriteLine($"{playlistExporter.Name}: {ex.ToString()}");
                        }
                    }
                    foreach (IItemExporter itemExporter in YoutubePlaylistExporterSettings.ItemExporters)
                    {
                        try
                        {
                            itemExporter.Export(currentPlaylist.PlaylistItems, currentPlaylist.Name);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Encountered exception with an item exporter.");
                            Console.WriteLine($"{itemExporter.Name}: {ex.ToString()}");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception while exporting playlist with id {playlistId}.");
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Calls the GetPlaylistItemsResponse method until it gets all the items in the playlist.
        /// </summary>
        public Playlist GetPlaylistAllItems(string playlistId)
        {
            Playlist youtubePlaylist = new Playlist();
            youtubePlaylist.Id = playlistId;
            int attempts = 0;
            string nextPageToken = null;
            while (attempts < YoutubePlaylistExporterSettings.MaximumAPICalls)
            {
                attempts++;
                var tempResponse = GetPlaylistItemsResponse(playlistId, nextPageToken);
                if (attempts == 1)
                {
                    youtubePlaylist.TotalItems = tempResponse.PageInfo.TotalResults;
                }
                foreach (Item item in tempResponse.Items)
                {
                    PlaylistItem playlistItem = item.ToPlaylistItem();
                    playlistItem.DateTimeAdded = DateTimeOffset.Now;
                    youtubePlaylist.PlaylistItems.Add(playlistItem);
                }
                nextPageToken = tempResponse.NextPageToken;
                if (youtubePlaylist.PlaylistItems.Count >= youtubePlaylist.TotalItems ||
                    string.IsNullOrWhiteSpace(nextPageToken)) break;
            }
            return youtubePlaylist;
        }

        /// <summary>
        /// Returns an instance of the Playlist class with the PlaylistItems property having the specified number of items.
        /// </summary>
        /// <returns></returns>
        public Playlist GetPlaylist(int numberOfItems, string playlistId)
        {
            Playlist youtubePlaylist = new Playlist();
            youtubePlaylist.Id = playlistId;
            int attempts = 0;
            string nextPageToken = null;
            while (youtubePlaylist.PlaylistItems.Count < numberOfItems && attempts < YoutubePlaylistExporterSettings.MaximumAPICalls)
            {
                attempts++;
                var tempResponse = GetPlaylistItemsResponse(playlistId, nextPageToken);
                if (attempts == 1)
                {
                    youtubePlaylist.TotalItems = tempResponse.PageInfo.TotalResults;
                }
                foreach (Item item in tempResponse.Items)
                {
                    PlaylistItem playlistItem = item.ToPlaylistItem();
                    playlistItem.DateTimeAdded = DateTimeOffset.Now;
                    youtubePlaylist.PlaylistItems.Add(playlistItem);
                }
                nextPageToken = tempResponse.NextPageToken;
                if (youtubePlaylist.PlaylistItems.Count >= numberOfItems ||
                    string.IsNullOrWhiteSpace(nextPageToken))
                {
                    youtubePlaylist.PlaylistItems = youtubePlaylist.PlaylistItems.GetRange(0, numberOfItems);
                };
            }
            return youtubePlaylist;
        }

        /// <summary>
        /// Calls the playlistItems endpoint of Youtube's API (youtube/v3/playlistItems). This endpoint does not provide the Playlist item with the property Name.
        /// </summary>
        /// <param name="playlistId">The id of the playlist.</param>
        /// <param name="nextPageToken">Token returned from previous PlaylistItemResponse call. Will return the first page if left null</param>
        /// <param name="itemsPerPage">50 is the maximum value (non-inclusive).</param>
        public PlaylistItemsResponse GetPlaylistItemsResponse(string playlistId,
            string nextPageToken = null, int itemsPerPage = 50)
        {

            string url = "https://www.googleapis.com/youtube/v3/playlistItems?" +
                        "part=snippet" +
                        $"&maxResults={itemsPerPage}" +
                        $"&playlistId={playlistId}" +
                        $"&key={YoutubePlaylistExporterSettings.ApiKey}";

            if (!string.IsNullOrWhiteSpace(nextPageToken))
            {
                url += $"&pageToken={nextPageToken}";
            }

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<PlaylistItemsResponse>(response.Content);
        }

        /// <summary>
        /// Calls the youtube/v3/playlists endpoint to returns a list of Playlists whose only set attributes are Id and Name.
        /// </summary>
        public List<Playlist> GetPlaylistsWithNames(List<string> playlistIds)
        {
            List<Playlist> playlists = new List<Playlist>();
            var client = new RestClient("https://www.googleapis.com/youtube/v3/playlists?part=snippet" +
                $"&id={String.Join(",", playlistIds)}&maxResults=25&key={YoutubePlaylistExporterSettings.ApiKey}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);
            Youtube.Playlists.PlaylistsResponse playlistsResponse =
                JsonConvert.DeserializeObject<Youtube.Playlists.PlaylistsResponse>(response.Content);
            foreach (Youtube.Playlists.Item item in playlistsResponse.Items)
            {
                playlists.Add(new Playlist { Id = item.Id, Name = item.Snippet.Title });
            }
            return playlists;
        }



    }

}
