using System;
using System.Collections.Generic;
using System.Net;
using SynologyAPI.Exception;
using SynologyRestDAL.Vs;
//using InfoResult = SynologyRestDAL.Vs.InfoResult;

namespace SynologyAPI
{
    public sealed class VideoStation : Station
    {
        private TvshowResult _tvshowresult;

        public VideoStation()
        {
        }

        public VideoStation(Uri url, string username, string password, WebProxy proxy)
            : base(url, username, password, proxy)
        {
        }

        public TvshowResult Shows
        {
            get { return _tvshowresult ?? RefreshTvShows(); }
            set
            {
                _tvshowresult = value;
                TvShow.Shows = value.Data.TvShows;
            }
        }

        protected override Dictionary<string, int> GetImplementedApi()
        {
            return _implementedApi ?? (_implementedApi = new Dictionary<string, int>
            {
                {"SYNO.API.Auth", 3},
                {"SYNO.VideoStation.TVShow", 2},
                {"SYNO.VideoStation.Info", 1},
                {"SYNO.VideoStation.TVShowEpisode", 2},
                {"SYNO.VideoStation.Library", 1}
            });
        }

        private TvshowResult RefreshTvShows()
        {
            const string additional =
                @"[""poster_mtime"",""summary"",""watched_ratio"",""file"",""director"",""genre""]";
            Shows = CallMethod<TvshowResult>("SYNO.VideoStation.TVShow", "list", new ReqParams
            {
                {"additional", additional},
                {"offset", 0.ToString()},
                {"sort_by", "added"},
                {"library_id", 0.ToString()}
            });
            return Shows;
        }

        public TvEpisodesInfo FindEpisodes(TvShow show)
        {
            const string additional = @"[""summary"",""collection"",""poster_mtime"",""watched_ratio""]";
            var tvEpisodesResult = CallMethod<TvEpisodesResult>("SYNO.VideoStation.TVShowEpisode", "list", new ReqParams
            {
                {"additional", additional},
                {"tvshow_id", show.Id.ToString()},
                {"library_id", 0.ToString()}
            });
            if (!tvEpisodesResult.Success)
                throw new TvEpisodeRequestException(@"Synology error code " + tvEpisodesResult.Error);
            return tvEpisodesResult.Data;
        }

        public ListResult List(string[] additional, int offset = 0, int limit = -1)
        {
            return List(String.Join(",", additional), offset, limit);
        }

        public ListResult List(string additional, int offset = 0, int limit = -1)
        {
            return CallMethod<ListResult>("SYNO.VideoStation.Library", "list");
        }
    }
}