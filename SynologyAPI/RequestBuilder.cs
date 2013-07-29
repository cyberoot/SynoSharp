using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class ReqParams : Dictionary<string, string>
    {
        
    }

    public class RequestBuilder
    {
        private ReqParams _reqData = new ReqParams()
            {
                {"api", "SYNO.API.Info"},
                {"cgiPath", "query.cgi"},
                {"version", "1"},
                {"method", "query"},
                {"sid", String.Empty}
            };
        private ReqParams _params = new ReqParams();

        private readonly string[] _headBuildOrder = { "api", "version", "method" };

        public RequestBuilder()
        {}

        public RequestBuilder(string sessionId)
            : this()
        {
            Session(sessionId);
        }

        public RequestBuilder Api(string apiName)
        {
            _reqData["api"] = apiName;
            return this;
        }

        public RequestBuilder CgiPath(string path)
        {
            _reqData["cgiPath"] = path;
            return this;
        }

        public RequestBuilder Version(string version)
        {
            _reqData["version"] = version;
            return this;
        }

        public RequestBuilder Method(string method)
        {
            _reqData["method"] = method;
            return this;
        }

        public RequestBuilder Method(string method, ReqParams args)
        {
            _reqData["method"] = method;
            SetParams(args);
            return this;
        }

        public RequestBuilder Session(string sid)
        {
            _reqData["sid"] = sid;
            return this;
        }

        public RequestBuilder AddParam(string key, string value)
        {
            _params.Add(key, value);
            return this;
        }

        public ReqParams Params
        {
            get { return _params; }
            set { _params = value; }
        }

        public RequestBuilder SetParams(ReqParams newParams)
        {
            Params = newParams;
            return this;
        }

        public override string ToString()
        {
            return _build();
        }

        private string _build()
        {
            var request = new StringBuilder();
            request.Append("webapi");
            if (_reqData["cgiPath"] != String.Empty)
            {
                request.Append("/" + _reqData["cgiPath"] + "?");
            }
            var reqHead = (from s in _headBuildOrder where _reqData[s] != String.Empty select s + "=" + _reqData[s]).ToList();
            if (reqHead.Any())
            {
                request.Append(String.Join("&", reqHead));
            }
            var reqParams = _params.Select(param => param.Key + "=" + param.Value).ToList();
            if (reqParams.Any())
            {
                request.Append("&" + String.Join("&", reqParams));
            }
            if (_reqData["sid"] != String.Empty)
            {
                request.Append("&_sid=" + _reqData["sid"]);
            }
            return request.ToString();
        }

    }
}
