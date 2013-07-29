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
        protected override string[] ImplementedApi
        {
            get
            {
                return new[] { "SYNO.API.Auth", "SYNO.DownloadStation.Task", "SYNO.DownloadStation.Info" };
            }
        }

        public DownloadStation()
            : base()
        {}

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

        public ListResult GetTasks(string[] taskIds, string[] additional)
        {
            return GetTasks(String.Join(",", taskIds), String.Join(",", additional));
        }

        public ListResult GetTasks(string taskIds, string additional = "detail")
        {
            return CallMethod<ListResult>("SYNO.DownloadStation.Task",
                "getinfo", new ReqParams()
                    {
                        {"id", taskIds},
                        {"additional", additional}
                    }
            );
        }

        public SynologyRestDAL.TResult<Object> CreateTask(string url)
        {
            return CallMethod<TResult<Object>>("SYNO.DownloadStation.Task",
                "create", new ReqParams() 
                {
                    {"uri", url}
                }
            );
        }

        public SynologyRestDAL.TResult<Object> CreateTask(string fileName, Stream fileStream)
        {
            return PostFile<TResult<Object>>("SYNO.DownloadStation.Task", "create", fileName, fileStream);
        }

    }
}
