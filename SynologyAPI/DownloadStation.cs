using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SynologyRestDAL;
using SynologyRestDAL.Ds;
using System.IO;

namespace SynologyAPI
{
    public sealed class DownloadStation : Station
    {
        public DownloadStation()
            : base()
        {}

        protected override Dictionary<string, int> GetImplementedApi()
        {
            return _implementedApi ?? (_implementedApi = new Dictionary<string, int>() { { "SYNO.API.Auth", 3 }, { "SYNO.DownloadStation.Task", 3 }, { "SYNO.DownloadStation.Info", 3 } });
        }

        public DownloadStation(Uri url, string username, string password, WebProxy proxy)
            : base(url, username, password, proxy)
        {
        }

        public DownloadStation(Uri url, string username, string password)
            : base(url, username, password)
        {
        }

        public InfoResult Info()
        {
            return CallMethod<InfoResult>("SYNO.DownloadStation.Info", "getinfo");
        }

        public ListResult List()
        {
            return CallMethod<ListResult>("SYNO.DownloadStation.Task", "list");
        }

        public ListResult List(string[] additional, int offset = 0, int limit = -1)
        {
            return List(String.Join(",", additional), offset, limit);
        }

        public ListResult List(string additional, int offset = 0, int limit = -1)
        {
            return CallMethod<ListResult>("SYNO.DownloadStation.Task",
                "list", new ReqParams
                    {
                        {"additional", additional},
                        {"offset", offset.ToString()},
                        {"limit", limit.ToString()}
                    }
           );
        }

        public TaskOperationResult PauseTasks(string[] taskIds)
        {
            return PauseTasks(String.Join(",", taskIds));
        }

        public TaskOperationResult PauseTasks(string taskIds)
        {
            return CallMethod<TaskOperationResult>("SYNO.DownloadStation.Task",
                "pause", new ReqParams
                {
                        {"id", taskIds},
                    }
            );
        }

        public TaskOperationResult ResumeTasks(string[] taskIds)
        {
            return ResumeTasks(String.Join(",", taskIds));
        }

        public TaskOperationResult ResumeTasks(string taskIds)
        {
            return CallMethod<TaskOperationResult>("SYNO.DownloadStation.Task",
                "resume", new ReqParams
                {
                        {"id", taskIds},
                    }
            );
        }

        public TaskOperationResult DeleteTasks(string[] taskIds, bool forceComplete = false)
        {
            return DeleteTasks(String.Join(",", taskIds), forceComplete);
        }

        public TaskOperationResult DeleteTasks(string taskIds, bool forceComplete = false)
        {
            return CallMethod<TaskOperationResult>("SYNO.DownloadStation.Task",
                "delete", new ReqParams
                {
                        {"id", taskIds},
                        {"force_complete", forceComplete == null ? "false" : forceComplete.ToString().ToLower()}
                    }
            );
        }

        public ListResult GetTasks(string[] taskIds, string[] additional)
        {
            return GetTasks(String.Join(",", taskIds), String.Join(",", additional));
        }

        public ListResult GetTasks(string taskIds, string additional = "detail")
        {
            return CallMethod<ListResult>("SYNO.DownloadStation.Task",
                "getinfo", new ReqParams
                {
                        {"id", taskIds},
                        {"additional", additional}
                    }
            );
        }

        public TResult<Object> CreateTask(string url)
        {
            return CallMethod<TResult<Object>>("SYNO.DownloadStation.Task",
                "create", new ReqParams
                {
                    {"uri", url}
                }
            );
        }

        public TResult<Object> CreateTask(string fileName, Stream fileStream)
        {
            return PostFile<TResult<Object>>("SYNO.DownloadStation.Task", "create", fileName, fileStream);
        }

    }
}
