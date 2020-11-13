using System;

namespace YoutubePlaylistExporter
{
    partial class Program
    {

        static void Main(string[] args)
        {
            try
            {
                //Instance the YoutubePlaylistExporter class
                YoutubePlaylistExporter youtubePlaylistExporter = new YoutubePlaylistExporter();

                //Write the ids of the loaded public playlist from the YoutubeExporterSettings.json file
                Console.WriteLine("Exporting playlists with Ids: {0}",
                    String.Join(",", youtubePlaylistExporter.YoutubePlaylistExporterSettings.PublicPlaylistIds));

                //Export the public playlists.
                youtubePlaylistExporter.ExportPublicPlaylists();

                Console.WriteLine("Finished exporting.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }

    }
}
