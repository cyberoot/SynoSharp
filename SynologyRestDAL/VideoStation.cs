using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
        public class TvLibraryInfo
        {
            [DataMember(Name = "offset")]
            public int Offset { get; set; }
            [DataMember(Name = "total")]
            public int Total { get; set; }
            [DataMember(Name = "tvshows")]
            public IEnumerable<TvShow> TvShows { get; set; }
        }

        [DataContract]
        public class TvShow
        {
            private DateTime? _originalAvailable;

            [DataMember(Name = "id")]
            public int Id { get; set; }
            [DataMember(Name = "mapper_id")]
            public int MapperId { get; set; }
            [DataMember(Name = "metadata_locked")]
            public bool MetadataLocked { get; set; }
            [DataMember(Name = "original_available")]
            private string OriginalAvailableSetter {
                set
                {
                    var dateParts = value.Split('-');
                    if (dateParts.Length < 3) return;
                    _originalAvailable = new DateTime(Int32.Parse(dateParts[0]), Int32.Parse(dateParts[1]), Int32.Parse(dateParts[2])); 
                }
                get
                {
                    return _originalAvailable == null ? null : string.Format("{0}-{1}-{2}", _originalAvailable.Value.Year, _originalAvailable.Value.Month,
                        _originalAvailable.Value.Day);
                }
            }

            public DateTime? OriginalAvailable => _originalAvailable;

            [DataMember(Name = "sort_title")]
            public string SortTitle { get; set; }
            [DataMember(Name = "title")]
            public string Title { get; set; }

            public override string ToString()
            {
                return string.Format("Id: {0}, MapperId: {1}, MetadataLocked: {2}, OriginalAvailable: {3}, SortTitle: {4}, Title: {5}", Id, MapperId, MetadataLocked, OriginalAvailable, SortTitle, Title);
            }
        }

        [DataContract]
        public class Library
        {
            [DataMember(Name="id")]
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
