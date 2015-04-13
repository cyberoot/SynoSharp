using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynologyAPI;
using SynologyRestDAL.Vs;

namespace SynologyApiTest.VideoStationTests
{
    [TestClass]
    public class LibraryListTests : BaseSynologyTests
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
            var tvShowsList = result.Data.TvShows.ToList();

            int longest = tvShowsList.OrderByDescending(t => t.Title.Length).First().Title.Length;
            var tvShows = tvShowsList.OrderBy(t => t.SortTitle);

            foreach (TvShow show in tvShows)
            {
                Debug.WriteLine(string.Format("#{0} | {1} {3}| {2}",
                    show.Id, show.Title, show.OriginalAvailable,
                    new String(' ', longest-show.Title.Length)));
            }

            Assert.IsTrue(result.Success);

        }

        private VideoStation VideoStation
        {
            get
            {
                var vs = new VideoStation(new Uri(Host), Username, Password, CreateProxy(Proxy));
                return vs.Login() ? vs : null;
            }
        }

        
    }
}
