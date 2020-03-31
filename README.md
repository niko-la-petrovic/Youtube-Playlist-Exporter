# Youtube-Playlist-Exporter
A cross-platform .NET Core solution for exporting (currently only public) playlists from Youtube.

# Latest Notes
- The program/code can be used to export data about playlists or playlist videos obtained from Youtube's v3 API.
- The current implementation can export **only public playlists.** This is a feature to add. 
- The current implementation works by accessing Youtube's API. Therefore **you must create your own API key** at the Google Developers Console. You can follow [this](https://developers.google.com/youtube/v3/getting-started) article if you are not familiar with it.
- I will work on making the documentation more complete.
- Only Windows binaries are avaliable as of now.
- I plan on making this into a NuGet package.

# How to use as is

### Download the binaries 
TLDR: You can find them here [here](https://github.com/niko-la-petrovic/Youtube-Playlist-Exporter/releases). See the configuring section below and run the program afterwards.

 The binaries will also contain the **YoutubeExporterSettings.json** file. Since this is a .NET Core app, you can choose to download the self-contained version and not have to install the .NET Core Runtime, or a smaller runtime-dependent version. 
### Configuring
The **YoutubeExporterSettings.json** file contains configurable options that you can use to change the exporting behavior.* The file comes with the bianries.

Use the following fields of the settings file to insert your Google API Key (see Notes above) and Youtube playlist ids. You can find those in the URL to your playlist or a video from it. The playlist id is the value of the query string list, i.e. https://youtube.com/...?list=**playlistid...**
```json 
    "publicPlaylistIds": [ "playlistId1", "playlistId2" ],
    "apiKey": "GoogleAPIKey",
```

A complete explanation of all the modifiable options is to be written.

*The changes in the settings are reflected when the program is re-run.
### Building

To build the project yourself, you require the one of the .NET Core runtimes.

---

# Motivation for making the project

I use Youtube for keeping a list of favorite songs, which occasionally get deleted for various reasons and it upsets me to lose a song. Therefore, you can use this to keep a persistent record of your favorite songs.


# Using the code:

The Program.cs shows a simple way of instancing the YoutubeExporter class and its use.

You can see my implementation of the DefaultPlaylistExporter and DefaultItemExporter classes to get an idea, or you can just implement one of the interfaces (IExporter, IItemExporter or IPlaylistExporter). You can add an instance of your interface to the PlaylistExporters or ItemExporters properties of the YoutubeExporterSettings class, depending on your chosen interface. 


If you would like to use the existing way to inject settings into your interface from the **YoutubeExporterSettings.json** file, you can add your own exporter with its properties to the exporters array in the settings file.

Currently, unless the property exporter property `defaultImplementation` is not set to true, that exporter will not be instanced, and you have to replace the existing instance with your own after you obtain the settings from it, but I will find a way to get around this with abstractions.
