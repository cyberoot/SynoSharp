using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SynologyRestDAL;
using SynologyRestDAL.Ds;
using System.IO;
using SynologyRestDAL.Vs;

//using InfoResult = SynologyRestDAL.Vs.InfoResult;
using ListResult = SynologyRestDAL.Vs.ListResult;
using TaskOperationResult = SynologyRestDAL.Ds.TaskOperationResult;

namespace SynologyAPI
{
    public sealed class VideoStation : Station
    {
        public VideoStation()
            : base()
        {}

        protected override Dictionary<string, int> GetImplementedApi()
        {
            return _implementedApi ?? (_implementedApi = new Dictionary<string, int>() {
                { "SYNO.API.Auth", 3 },
                { "SYNO.VideoStation.TVShow", 2 },
                { "SYNO.VideoStation.Info", 1 },
                { "SYNO.VideoStation.Library", 1 }
            });
        }

        public VideoStation(Uri url, string username, string password, WebProxy proxy)
            : base(url, username, password, proxy)
        {
        }

        public VideoStation(Uri url, string username, string password)
            : base(url, username, password)
        {
        }

        public InfoResult Info()
        {
            return CallMethod<InfoResult>("SYNO.VideoStation.Info", "getinfo");
        }

        public ListResult List()
        {
            return CallMethod<ListResult>("SYNO.VideoStation.Library", "list");
        }

        public TvshowResult TVShows()
        {
            string additional = @"[""poster_mtime"",""summary"",""watched_ratio"",""file"",""director"",""genre""]";
            return CallMethod<TvshowResult>("SYNO.VideoStation.TVShow", "list", new ReqParams
            {
                {"additional", additional },
                {"offset", 0.ToString() },
                {"sort_by", "added" },
                {"library_id", 0.ToString() },

            });
        }

        public SynologyRestDAL.Vs.ListResult List(string[] additional, int offset = 0, int limit = -1)
        {
            return List(String.Join(",", additional), offset, limit);
        }

        public ListResult List(string additional, int offset = 0, int limit = -1)
        {
            return CallMethod<ListResult>("SYNO.VideoStation.Library",
                "list", new ReqParams
                    {
//                        {"additional", additional},
//                        {"offset", offset.ToString()},
//                        {"limit", limit.ToString()}
                    }
           );
        }

        public TaskOperationResult PauseTasks(string[] taskIds)
        {
            return PauseTasks(String.Join(",", taskIds));
        }

        public TaskOperationResult PauseTasks(string taskIds)
        {
            return CallMethod<TaskOperationResult>("SYNO.VideoStation.Task",
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
            return CallMethod<TaskOperationResult>("SYNO.VideoStation.Task",
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
            return CallMethod<TaskOperationResult>("SYNO.VideoStation.Task",
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
            return CallMethod<ListResult>("SYNO.VideoStation.Task",
                "getinfo", new ReqParams
                {
                        {"id", taskIds},
                        {"additional", additional}
                    }
            );
        }

        public TResult<Object> CreateTask(string url)
        {
            return CallMethod<TResult<Object>>("SYNO.VideoStation.Task",
                "create", new ReqParams
                {
                    {"uri", url}
                }
            );
        }

        public TResult<Object> CreateTask(string fileName, Stream fileStream)
        {
            return PostFile<TResult<Object>>("SYNO.VideoStation.Task", "create", fileName, fileStream);
        }

    }
}
