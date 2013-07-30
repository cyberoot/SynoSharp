SynoSharp
=========

C# Implementation of official Synology Web API

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

