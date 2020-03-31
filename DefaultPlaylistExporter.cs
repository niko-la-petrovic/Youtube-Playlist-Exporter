using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using YoutubePlaylistExporter.Interfaces;

namespace YoutubePlaylistExporter
{
    class DefaultPlaylistExporter : IPlaylistExporter
    {
        public string Name { get; set; }
        public string FolderName { get; set; }
        public string Type { get; set; }
        public bool OverwriteExisting { get; set; }
        public bool SortDescending { get; set; } = true;
        public string FileName { get; set; } = "export.json";
        public string CombinedFolderName { get; set; }

        /// <summary>
        /// Exports the playlist to a json file in the CombinedFolderName directory. This property will contain the name of the playlist if that name is a valid path.
        /// </summary>
        /// <remarks>This method potentially modifies the argument playlist and then returns it. (This is so that items in the playlist that might have been deleted/made priavte can be kept.)</remarks>
        public Playlist Export(Playlist playlist)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(playlist.Name) || !playlist.Name.Any(Path.GetInvalidPathChars().Contains))
                {
                    CombinedFolderName = Path.Join(playlist.Name, FolderName);
                }
                else
                {
                    CombinedFolderName = FolderName;
                }

                if (OverwriteExisting)
                {
                    Utility.SortListPlaylistItems(playlist.PlaylistItems, SortDescending);

                    WriteToFile(playlist, FileName);
                }
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(CombinedFolderName);
                    if (!directory.Exists)
                    {
                        directory.Create();
                    }
                    FileInfo[] fileInfos = directory.GetFiles("*.json");
                    string newFileName = Path.GetFileNameWithoutExtension(FileName) +
                            DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Path.GetExtension(FileName);

                    //Needed to retain old videos (perhaps deleted or made private in the meantime)
                    if (fileInfos.Length > 0)
                    {
                        Playlist lastPlaylist = JsonConvert.DeserializeObject<Playlist>(
                            File.ReadAllText(fileInfos.OrderBy(f => f.LastAccessTime).First().FullName));
                        foreach (PlaylistItem playlistItem in playlist.PlaylistItems)
                        {
                            if (!lastPlaylist.PlaylistItems.Exists(i => i.Snippet.ResourceId.VideoId == playlistItem.Snippet.ResourceId.VideoId))
                            {
                                lastPlaylist.PlaylistItems.Add(playlistItem);
                                lastPlaylist.TotalItems++;
                            }
                        }
                        playlist = lastPlaylist;

                    }

                    Utility.SortListPlaylistItems(playlist.PlaylistItems, SortDescending);

                    WriteToFile(playlist, newFileName);

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return playlist;
        }

        /// <summary>
        /// Serializes the playlist into json format for writing to a file.
        /// </summary>
        /// <param name="playlist">Instance of Playlist to write to file.</param>
        /// <param name="fileName">The name (not path) of the file to which to write.</param>
        public void WriteToFile(Playlist playlist, string fileName)
        {
            Directory.CreateDirectory(CombinedFolderName);
            File.WriteAllText(Path.Join(CombinedFolderName, fileName), JsonConvert.SerializeObject(playlist));
        }

    }
}
