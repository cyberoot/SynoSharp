using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SynologyRestDAL
{
    [DataContract]
    public class ErrorCode
    {
        [DataMember(Name = "code")]
        public int Code;
    }

    [DataContract]
    public class TResult<T>
    {
        [DataMember(Name = "data")]
        public T Data  { get; set; }
        [DataMember(Name = "success")]
        public bool Success { get; set; }
        [DataMember(Name = "error")]
        public ErrorCode Error { get; set; }
    }

    [DataContract]
    public class ApiSpec
    {
        [DataMember(Name = "maxVersion")]
        public int MaxVersion { get; set; }
        [DataMember(Name = "minVersion")]
        public int MinVersion { get; set; }
        [DataMember(Name = "path")]
        public string Path { get; set; }
    }

    [DataContract]
    public class ApiInfo : TResult<Dictionary<string, ApiSpec>>
    {
    
    }

    [DataContract]
    public class SidContainer
    {
        [DataMember(Name = "sid")]
        public string Sid { get; set; }
    }

    [DataContract]
    public class LoginResult : TResult<SidContainer>
    {

    }

}
