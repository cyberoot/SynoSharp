using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SynologyRestDAL
{
    namespace Ds
    {
        [DataContract]
        public class Info
        {
            [DataMember(Name = "is_manager")]
            public bool IsManager { get; set; }
            [DataMember(Name = "version")]
            public int Version { get; set; }
            [DataMember(Name = "version_string")]
            public string VersionString { get; set; }

        }

        [DataContract]
        public class InfoResult : TResult<Info>
        {

        }

        [DataContract]
        public class TaskPeer
        {
            [DataMember(Name = "address")]
            public string Address { get; set; }
            [DataMember(Name = "agent")]
            public string Agent { get; set; }
            [DataMember(Name = "progress")]
            public string Progress { get; set; }
            [DataMember(Name = "speed_download")]
            public int SpeedDownload { get; set; }
            [DataMember(Name = "speed_upload")]
            public int SpeedUpload { get; set; }
        }

        [DataContract]
        public class TaskTracker
        {
            [DataMember(Name = "url")]
            public string Url { get; set; }
            [DataMember(Name = "status")]
            public string Status { get; set; }
            [DataMember(Name = "update_timer")]
            public int UpdateTimer { get; set; }
            [DataMember(Name = "seeds")]
            public int Seeds { get; set; }
            [DataMember(Name = "peers")]
            public int Peers { get; set; }
        }

        [DataContract]
        public class TaskFile
        {
            [DataMember(Name = "filename")]
            public string Filename { get; set; }
            [DataMember(Name = "size")]
            public string Size { get; set; }
            [DataMember(Name = "size_downloaded")]
            public string SizeDownloaded { get; set; }
            [DataMember(Name = "priority")]
            public string Priority { get; set; }
        }

        [DataContract]
        public class TaskDetail
        {
            [DataMember(Name = "destination")]
            public string Destination { get; set; }
            [DataMember(Name = "uri")]
            public string Uri { get; set; }
            [DataMember(Name = "creater_time")]
            public string CreateTime { get; set; }
            [DataMember(Name = "priority")]
            public string Priority { get; set; }
            [DataMember(Name = "total_peers")]
            public int TotalPeers { get; set; }
            [DataMember(Name = "connected_peers")]
            public int ConnectedPeers { get; set; }
            [DataMember(Name = "connected_leechers")]
            public int ConnectedLeechers { get; set; }
        }

        [DataContract]
        public class TaskTransfer
        {
            [DataMember(Name = "size_downloaded")]
            public string SizeDownloaded { get; set; }
            [DataMember(Name = "size_uploaded")]
            public string SizeUploaded { get; set; }
            [DataMember(Name = "speed_download")]
            public int SpeedDownload { get; set; }
            [DataMember(Name = "speed_upload")]
            public int SpeedUpload { get; set; }
        }

        [DataContract]
        public class TaskStatusExtra
        {
            [DataMember(Name = "error_detail")]
            public string ErrorDetail { get; set; }
            [DataMember(Name = "unzip_progress")]
            public int UnzipProgress { get; set; }
        }

        [DataContract]
        public class Additional
        {
            [DataMember(Name = "detail")]
            public TaskDetail Detail { get; set; }
            [DataMember(Name = "transfer")]
            public TaskTransfer Transfer { get; set; }
            [DataMember(Name = "file")]
            public IEnumerable<TaskFile> File { get; set; }
            [DataMember(Name = "tracker")]
            public IEnumerable<TaskTracker> Tracker { get; set; }
            [DataMember(Name = "peer")]
            public IEnumerable<TaskPeer> Peer { get; set; }

        }

        [DataContract]
        public class Task
        {
            [DataMember(Name = "id")]
            public string Id { get; set; }
            [DataMember(Name = "type")]
            public string Type { get; set; }
            [DataMember(Name = "username")]
            public string Username { get; set; }
            [DataMember(Name = "title")]
            public string Title { get; set; }
            [DataMember(Name = "size")]
            public string Size { get; set; }
            [DataMember(Name = "status")]
            public string Status { get; set; }
            [DataMember(Name = "status_extra")]
            public TaskStatusExtra StatusExtra { get; set; }
            [DataMember(Name = "additional")]
            public Additional Additional { get; set; }

            public override string ToString()
            {
                return base.ToString();
            }
        }


        [DataContract]
        public class List
        {
            [DataMember(Name = "offeset")] // typo, but not here, in API actually (((
            public int Offset { get; set; }
            [DataMember(Name = "tasks")]
            public IEnumerable<Task> Tasks { get; set; }
            [DataMember(Name = "total")]
            public int Total { get; set; }
        }

        [DataContract]
        public class ListResult : TResult<List>
        {

        }

        [DataContract]
        public class SingleTaskOperationResult
        {
            [DataMember(Name = "error")]
            public int Error { get; set; }

            [DataMember(Name = "id")]
            public string Id { get; set; }
        }

        [DataContract]
        public class TaskOperationResult : TResult<IEnumerable<SingleTaskOperationResult>>
        {

        }

    }
}
