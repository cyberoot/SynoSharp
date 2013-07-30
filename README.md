---
published: false
---

SynoSharp
=========

C# Implementation of official Synology Web API

Synology API
---------

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

synods Download Station CLI interface
---------

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
