# SynoSharp

C# Implementation of official Synology Web API

[![Build Status](https://travis-ci.org/robinkanters/SynoSharp.svg?branch=master)](https://travis-ci.org/robinkanters/SynoSharp)

## Synology API

### Download Station

Yet under construction, the base classes can be easily used to talk to Synology API purely in C# way, e.g.

```csharp
// ...
var ds = new DownloadStation(
									new Uri(appSettings["host"]),
									appSettings["username"],
									appSettings["password"],
									CreateProxy(appSettings["proxy"])
);
var taskList = from task in ds.List().Data.Tasks select task;
// ...
```

### Video Station

The current implementation supports getting information on:

- Tv shows
  - Episodes

#### Examples

##### List Tv series in library

```csharp
string host = "https://yourdshost:5001/video", username = "myuser", password = "s3cr3t", proxy = string.Empty;
var vs = new VideoStation(new Uri(host), username, password, CreateProxy(proxy));

TvshowResult tvShows = vs.Shows.Data.TvShows;

foreach(TvShow show in tvShows)
{
	Debug.WriteLine(show.Id);
	Debug.WriteLine(show.Title);
	Debug.WriteLine(show.OriginalAvailable);
	// and more...
}
```

##### List episodes in a Tv show

```csharp
TvshowResult tvShows = vs.Shows.Data.TvShows.First();
var tvEpisodes = VideoStation.FindEpisodes(show).Episodes;

foreach(TvEpisode episode in tvEpisodes)
{
	Debug.WriteLine(episode.Episode); // Nth episode in season
	Debug.WriteLine(episode.Season);
	Debug.WriteLine(episode.Tagline);
	Debug.WriteLine(episode.Show); // the TvShow that the episode belongs to
	// and more...
}
```

## synods CLI interface

### Download Station

List all tasks with status downloading or waiting, include transfer info
```bat
synods list -s downloading waiting -a transfer
```
Upload .torrent file to create new download task
```bat
synods new --file SomeStuff.torrent
```

Create download task from URL
```bat
synods new --url http://download.some/stuff.zip
```
Pause download task
```bat
synods pause --id dbid_2420
```
