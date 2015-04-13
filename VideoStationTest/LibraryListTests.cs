using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynologyAPI;
using SynologyRestDAL.Vs;

namespace VideoStationTest
{
    [TestClass]
    public class LibraryListTests
    {
        [TestMethod]
        public void CanCreateVideoStationSession()
        {
            var vs = VideoStation;

            Assert.IsNotNull(vs);
        }

        [TestMethod]
        public void CanListVideosInLibrary()
        {
            var vs = VideoStation;

            TvshowResult result = vs.TVShows();
            var tvShows = result.Data.TvShows;
            Debug.WriteLine(tvShows);

            int longest = tvShows.OrderByDescending(t => t.Title.Length).First().Title.Length;
            tvShows = tvShows.OrderBy(t => t.SortTitle);

            foreach(TvShow l in tvShows)
            {
                Debug.WriteLine(string.Format("#{2} | {0} {3}| {1}",
                    l.Title, l.OriginalAvailable, l.Id,
                    new String(' ', longest-l.Title.Length)));
            }

            Assert.IsTrue(result.Success);

        }

        private static VideoStation VideoStation
        {
            get
            {
                const string host = "http://synologyhost", username = "", password = "", proxy = "";
                var vs = new VideoStation(new Uri(host), username, password, CreateProxy(proxy));
                return vs.Login() ? vs : null;
            }
        }

        private static WebProxy CreateProxy(string proxyUrl)
        {
            if (String.IsNullOrWhiteSpace(proxyUrl))
            {
                return null;
            }
            return new WebProxy(new Uri(proxyUrl));
        }

    }
}
