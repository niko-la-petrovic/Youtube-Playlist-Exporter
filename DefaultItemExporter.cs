using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YoutubePlaylistExporter.Interfaces;

namespace YoutubePlaylistExporter
{
    public class DefaultItemExporter : IItemExporter
    {
        public string IndividualItemFormat { get; set; }
        public bool DefaultImplementation { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string FolderName { get; set; }
        public bool OverwriteExisting { get; set; }
        public bool SortDescending { get; set; } = true;
        public string FileName { get; set; } = "export.txt";
        public string DateTimeFormat { get; set; } = "dd MMMM yyyy";
        public string CombinedFolderName { get; set; }

        /// <summary>
        /// Exports the playlistItems to a file in the CombinedFolderName directory. This property will contain the name of the playlist if that name is a valid path.
        /// </summary>
        /// <param name="playlistItems">The list of PlaylistItem instances to export.</param>
        /// <param name="playlistName">The name of the playlist. Should not contain invalid characters.</param>
        public void Export(List<PlaylistItem> playlistItems, string playlistName = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(playlistName) && !playlistName.Any(Path.GetInvalidPathChars().Contains))
                {
                    CombinedFolderName = Path.Join(playlistName, FolderName);
                }
                else
                {
                    CombinedFolderName = FolderName;
                }

                Utility.SortListPlaylistItems(playlistItems, SortDescending);
                if (OverwriteExisting)
                {
                    WriteToFile(playlistItems, FileName);
                }
                else
                {
                    WriteToFile(playlistItems, Path.GetFileNameWithoutExtension(FileName) +
                        DateTimeOffset.Now.ToUnixTimeSeconds() + Path.GetExtension(FileName));
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Creates a directory in the path of the CombinedFolderName property. Then for each item in the playlist writes the return value of PlaylistItemToString to the specified file name in that directory.
        /// </summary>
        /// <param name="playlistItems">List of PlaylistItem instances.</param>
        /// <param name="fileName">The name (not path) of the file to which to write.</param>
        public void WriteToFile(List<PlaylistItem> playlistItems, string fileName)
        {
            Directory.CreateDirectory(CombinedFolderName);
            using (FileStream fileStream = File.OpenWrite(Path.Join(CombinedFolderName, fileName)))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    for (int i = 0; i < playlistItems.Count; i++)
                    {
                        streamWriter.WriteLine(PlaylistItemToString(playlistItems[i], i, IndividualItemFormat));
                    }
                }
            }
        }

        public string PlaylistItemToString(PlaylistItem playlistItem, int i, string individualItemFormat)
        {
            string itemString = IndividualItemFormat;
            itemString = Regex.Replace(itemString, "{i}", (i + 1).ToString());
            itemString = Regex.Replace(itemString, "{title}", playlistItem.Snippet.Title.
                Replace("'", "").Replace("\"", ""));
            itemString = Regex.Replace(itemString, "{videourl}",
                $"youtube.com/watch?v={playlistItem.Snippet.ResourceId.VideoId}");
            itemString = Regex.Replace(itemString, "{dateaddedtoplaylist}",
                playlistItem.Snippet.PublishedAt.ToString(DateTimeFormat));
            itemString = Regex.Replace(itemString, "{dateaddedtofile}",
                playlistItem.DateTimeAdded.ToString(DateTimeFormat));
            return itemString;
        }

    }

}
