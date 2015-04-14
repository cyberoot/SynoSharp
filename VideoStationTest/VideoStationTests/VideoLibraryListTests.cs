using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynologyAPI;

namespace SynologyApiTest.VideoStationTests
{
    [TestClass]
    public class VideoLibraryListTests : BaseSynologyTests
    {
        private VideoStation VideoStation
        {
            get
            {
                var vs = new VideoStation(new Uri(Host), Username, Password, CreateProxy(Proxy));
                return vs.Login() ? vs : null;
            }
        }

        [TestMethod]
        public void CanCreateVideoStationSession()
        {
            Assert.IsNotNull(VideoStation);
        }

        [TestMethod]
        public void CanListVideosInLibrary()
        {
            var result = VideoStation.TvShows;
            var tvShowsList = result.Data.TvShows.ToList();

            var longest = tvShowsList.OrderByDescending(t => t.Title.Length).First().Title.Length;
            var tvShows = tvShowsList.OrderBy(t => t.SortTitle);

            foreach (var show in tvShows)
            {
                Debug.WriteLine("#{0} | {1} {3}| {2}", show.Id, show.Title, show.OriginalAvailable,
                    new String(' ', longest - show.Title.Length));
            }

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void TvShowList_ListContainsShows()
        {
            var result = VideoStation.TvShows.Data.TvShows.ToArray();

            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void TvShowList_ShowsHaveTitles()
        {
            var result = VideoStation.TvShows.Data.TvShows;

            Assert.IsNotNull(result.First().Title);
        }

        [TestMethod]
        public void TvShowList_ShowsHaveIds()
        {
            var result = VideoStation.TvShows.Data.TvShows;

            Assert.IsNotNull(result.First().Id);
        }

        [TestMethod]
        public void TvShowList_ShouldBeAbleToSort()
        {
            var result = VideoStation.TvShows.Data.TvShows.ToList();

            Assert.IsNotNull(result);

            var resultOrdered = result.OrderBy(s => s.SortTitle);
            var resultOrderedDesc = result.OrderByDescending(s => s.SortTitle);

            Assert.IsFalse(resultOrdered.SequenceEqual(resultOrderedDesc));

            Assert.IsFalse(resultOrdered.SequenceEqual(result));
            Assert.IsFalse(resultOrderedDesc.SequenceEqual(result));
        }
    }
}