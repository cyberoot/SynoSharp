using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SynologyRestDAL
{
    namespace Vs
    {
        [DataContract]
        public class ListResult : TResult<IEnumerable<Library>>
        {
        }

        [DataContract]
        public class TvshowResult : TResult<TvLibraryInfo>
        {
        }

        [DataContract]
        public class TvEpisodesResult : TResult<TvEpisodesInfo>
        {
        }

        [DataContract]
        public class TvInfo
        {
            [DataMember(Name = "total")]
            public int Total { get; set; }
        }

        [DataContract]
        public class TvLibraryInfo : TvInfo
        {
            [DataMember(Name = "tvshows")]
            public IEnumerable<TvShow> TvShows { get; set; }

            [DataMember(Name = "offset")]
            public int Offset { get; set; }
        }

        [DataContract]
        public class TvEpisodesInfo : TvInfo
        {
            [DataMember(Name = "episodes")]
            public IEnumerable<TvEpisode> Episodes { get; set; }
        }

        [DataContract]
        public class TvShow : TvItem
        {
            public static IEnumerable<TvShow> Shows { get; set; }
        }

        [DataContract]
        public abstract class TvItem
        {
            [DataMember(Name = "sort_title")]
            public string SortTitle { get; set; }

            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "metadata_locked")]
            public bool MetadataLocked { get; set; }

            [DataMember(Name = "id")]
            public int Id { get; set; }

            [DataMember(Name = "mapper_id")]
            public int MapperId { get; set; }

            public DateTime? OriginalAvailable { get; private set; }

            [DataMember(Name = "original_available")]
            private string OriginalAvailableSetter
            {
                set
                {
                    var dateParts = value.Split('-');
                    if (dateParts.Length < 3) return;
                    OriginalAvailable = new DateTime(Int32.Parse(dateParts[0]), Int32.Parse(dateParts[1]),
                        Int32.Parse(dateParts[2]));
                }
                get
                {
                    return OriginalAvailable == null
                        ? null
                        : string.Format("{0}-{1}-{2}", OriginalAvailable.Value.Year, OriginalAvailable.Value.Month,
                            OriginalAvailable.Value.Day);
                }
            }

            public override string ToString()
            {
                return
                    string.Format(
                        "SortTitle: {0}, Title: {1}, MetadataLocked: {2}, Id: {3}, MapperId: {4}, OriginalAvailable: {5}, OriginalAvailableSetter: {6}",
                        SortTitle, Title, MetadataLocked, Id, MapperId, OriginalAvailable, OriginalAvailableSetter);
            }
        }

        [DataContract]
        public class TvEpisode : TvItem
        {
            private TvShow _show;

            public TvEpisode(TvShow show)
            {
                Show = show;
            }

            public TvShow Show
            {
                get { return _show ?? (_show = TvShow.Shows.First(s => s.Id.Equals(TvshowId))); }
                set { _show = value; }
            }

            [DataMember(Name = "episode")]
            public int Episode { get; set; }

            [DataMember(Name = "last_watched")]
            public int LastWatched { get; set; }

            [DataMember(Name = "season")]
            public int Season { get; set; }

            [DataMember(Name = "tagline")]
            public string Tagline { get; set; }

            [DataMember(Name = "tvshow_id")]
            internal int TvshowId { get; set; }

            [DataMember(Name = "additional")]
            public TvShowAdditional Additional { private get; set; }

            public string Summary
            {
                get
                {
                    return Additional.Summary ?? string.Empty;
                }
            }

            public override string ToString()
            {
                return
                    string.Format(
                        "{{ Episode: {1}, LastWatched: {2}, Season: {3}, Tagline: {4}, Summary: {5}, Show: {{ {0} }} }}",
                        Show, Episode, LastWatched, Season, Tagline, Summary);
            }
        }

        [DataContract]
        public class TvShowAdditional
        {
            [DataMember(Name = "summary")]
            public string Summary { get; set; }

            public override string ToString()
            {
                return string.Format("Summary: {0}", Summary);
            }
        }

        [DataContract]
        public class Library
        {
            [DataMember(Name = "id")]
            public int Id { get; set; }

            [DataMember(Name = "is_public")]
            public bool IsPublic { get; set; }

            [DataMember(Name = "title")]
            public string Title { get; set; }

            public override string ToString()
            {
                return string.Format("Id: {0}, IsPublic: {1}, Title: {2}", Id, IsPublic, Title);
            }
        }
    }
}