namespace SynologyApiTest.VideoStationTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using SynologyAPI;
    using SynologyAPI.Exception;
    using SynologyRestDAL.Vs;
    using NUnit.Framework;

    [TestFixture]
    public class VideoLibraryListTests : BaseSynologyTests
    {
        [SetUp]
        [Test]
        public void Setup()
        {
            var vs = new VideoStation(new Uri(Host), Username, Password, CreateProxy(Proxy));
            VideoStation = vs.Login() ? vs : null;

            Assert.IsNotNull(VideoStation);
        }

        private VideoStation VideoStation { get; set; }

        [Test]
        public void CanCreateVideoStationSession()
        {
            Assert.IsNotNull(VideoStation);
        }

        [Test]
        public void CanListVideosInLibrary()
        {
            var result = VideoStation.Shows;
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

        [Test]
        public void TvShowList_ListContainsShows()
        {
            var result = VideoStation.Shows.Data.TvShows.ToArray();

            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public void TvShowList_ShowsHaveTitles()
        {
            var result = VideoStation.Shows.Data.TvShows;

            Assert.IsNotNull(result.First().Title);
        }

        [Test]
        public void TvShowList_ShowsHaveIds()
        {
            var result = VideoStation.Shows.Data.TvShows;

            Assert.IsNotNull(result.First().Id);
        }

        [Test]
        public void TvShowList_ShouldBeAbleToSort()
        {
            var result = VideoStation.Shows.Data.TvShows.ToList();

            Assert.IsNotNull(result);

            var resultOrdered = result.OrderBy(s => s.SortTitle);
            var resultOrderedDesc = result.OrderByDescending(s => s.SortTitle);

            Assert.IsFalse(resultOrdered.SequenceEqual(resultOrderedDesc));

            Assert.IsFalse(resultOrdered.SequenceEqual(result));
            Assert.IsFalse(resultOrderedDesc.SequenceEqual(result));
        }

        [Test]
        public void TvShowEpisode_ShouldListEpisodesForSeries()
        {
            var show = VideoStation.Shows.Data.TvShows.First();

            var tvEpisodes = VideoStation.FindEpisodes(show).Episodes;

            Assert.IsNotNull(tvEpisodes);
        }

        [Test]
        public void TvShowEpisode_EpisodeShouldHaveTitle()
        {
            var show = VideoStation.Shows.Data.TvShows.First();

            var episode = VideoStation.FindEpisodes(show).Episodes.First();

            Debug.WriteLine(episode.ToString());
        }

        [Test]
        public void TvShowEpisode_ShouldHaveShowInformation()
        {
            var show = VideoStation.Shows.Data.TvShows.First();

            var episode = VideoStation.FindEpisodes(show).Episodes.First(e => e.Summary.Length > 0);

            Debug.WriteLine(show.ToString());
            Debug.WriteLine(episode.ToString());

            Assert.AreEqual(show, episode.Show);

            Assert.IsNotNull(episode.Summary);
            Assert.IsNotNull(episode.Tagline);
        }

        [Test]
        public void TvShowEpisode_CanListEpisodesForShow()
        {
            var data = VideoStation.Shows;
            var show = data.Data.TvShows.First(s => s.Title.Contains(@"Mother"));

            var episodes = VideoStation.FindEpisodes(show).Episodes.ToList();
            var longestEpisodeLength = episodes.OrderByDescending(s => s.Tagline.Length).First().Tagline.Length;

            foreach (var episode in episodes)
            {
                var summary = string.Empty;
                if (episode.Summary != null)
                    summary = episode.Summary.Length > 40 ? episode.Summary.Substring(0, 40) : episode.Summary;

                Debug.WriteLine("#{0} | S{3}E{4} {1} {5}| {2}",
                    episode.Id,
                    episode.Tagline,
                    summary,
                    episode.Season < 10 ? "0" + episode.Season : episode.Season.ToString(),
                    episode.Episode < 10 ? "0" + episode.Episode : episode.Episode.ToString(),
                    new string(' ', longestEpisodeLength - episode.Tagline.Length));
            }

            Assert.IsTrue(data.Success);
        }

        [Test]
        [ExpectedException(exceptionType: typeof(InvalidDataException))]
        public void TvShowEpisode_ShouldGetExceptionIfSearchingEpisodesForNullShow()
        {
            var tvEpisodes = VideoStation.FindEpisodes(null).Episodes;
        }
    }
}