using System.Diagnostics;

namespace SynologyApiTest.VideoStationTests
{
    using System;
    using System.IO;
    using System.Linq;
    using SynologyAPI;
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

            Assert.That(VideoStation, Is.Not.Null);
        }

        private VideoStation VideoStation { get; set; }

        [Test]
        public void CanListVideosInLibrary()
        {
            var result = VideoStation.Shows;

#if DEBUG
            var tvShowsList = result.Data.TvShows.ToList();

            var longest = tvShowsList.OrderByDescending(t => t.Title.Length).First().Title.Length;
            var tvShows = tvShowsList.OrderBy(t => t.SortTitle);

            foreach (var show in tvShows)
            {
                Debug.WriteLine("#{0} | {1} {3}| {2}", show.Id, show.Title, show.OriginalAvailable,
                    new String(' ', longest - show.Title.Length));
            }
#endif

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data.TvShows.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void TvShowList_ListContainsShows()
        {
            var result = VideoStation.Shows.Data.TvShows.ToArray();

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        public void TvShowList_ShowsHaveTitles()
        {
            var result = VideoStation.Shows.Data.TvShows;

            Assert.That(result.First().Title, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void TvShowList_ShowsHaveIds()
        {
            var result = VideoStation.Shows.Data.TvShows;

            Assert.That(result.First().Id, Is.Not.Null.And.GreaterThan(0));
        }

        [Test]
        public void TvShowList_ShouldBeAbleToSort()
        {
            var result = VideoStation.Shows.Data.TvShows.ToList();

            Assert.That(result, Is.Not.Null);

            var resultOrdered = result.OrderBy(s => s.SortTitle);
            var resultOrderedDesc = result.OrderByDescending(s => s.SortTitle);

            Assert.That(result, Is.Not.Null.And.Not.Empty);
            Assert.That(resultOrdered, Is.Not.Null.And.Not.Empty);
            Assert.That(resultOrderedDesc, Is.Not.Null.And.Not.Empty);

            Assert.That(resultOrdered.SequenceEqual(result), Is.False);
            Assert.That(resultOrdered.SequenceEqual(resultOrderedDesc), Is.False);
            Assert.That(resultOrderedDesc.SequenceEqual(result), Is.False);
        }

        [Test]
        public void TvShowEpisode_ShouldListEpisodesForSeries()
        {
            var show = VideoStation.Shows.Data.TvShows.First();

            var tvEpisodes = VideoStation.FindEpisodes(show).Episodes;

            Assert.That(tvEpisodes, Is.Not.Null);
        }

        [Test]
        public void TvShowEpisode_EpisodeShouldHaveTitle()
        {
            var show = VideoStation.Shows.Data.TvShows.First();

            var episodes = VideoStation.FindEpisodes(show).Episodes;
            var firstEpisode = episodes.First();

            Assert.That(show,           Is.Not.Null);
            Assert.That(episodes,       Is.Not.Null.And.Not.Empty);
            Assert.That(firstEpisode,   Is.Not.Null);
        }

        [Test]
        public void TvShowEpisode_ShouldHaveShowInformation()
        {
            var show = VideoStation.Shows.Data.TvShows.First();

            var episode = VideoStation.FindEpisodes(show).Episodes.First(e => e.Summary.Length > 0);

            Assert.That(show, Is.EqualTo(episode.Show));

            Assert.That(episode.Summary, Is.Not.Null.And.Not.Empty);
            Assert.That(episode.Tagline, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        [TestCase(@"Mother")]
        [TestCase(@"Big Bang")]
        [TestCase(@"Broke Girls")]
        [TestCase(@"House of cards")]
        public void TvShowEpisode_CanListEpisodesForShow(string showTitle)
        {
            var data = VideoStation.Shows;

#if DEBUG
            var show = data.Data.TvShows.First(s => s.Title.IndexOf(showTitle, StringComparison.OrdinalIgnoreCase) >= 0);
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
#endif

            Assert.That(data.Success, Is.True);
        }

        [Test]
        [ExpectedException(exceptionType: typeof(InvalidDataException))]
        public void TvShowEpisode_ShouldGetExceptionIfSearchingEpisodesForNullShow()
        {
            var tvEpisodes = VideoStation.FindEpisodes(null).Episodes;
        }
    }
}