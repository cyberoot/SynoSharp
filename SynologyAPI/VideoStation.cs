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
            return CallMethod<ListResult>("SYNO.VideoStation.Library", "list");
        }
    }
}
