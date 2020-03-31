namespace YoutubePlaylistExporter.Interfaces
{
    public interface IPlaylistExporter : IExporter
    {
        public Playlist Export(Playlist playlist) { return playlist; }
    }
}
