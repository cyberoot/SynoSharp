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
            { "api", "SYNO.API.Info" },
            { "cgiPath", "query.cgi" },
            { "version", "1" },
            { "method", "query" },
            { "sid", String.Empty }
        };

        private ReqParams _params = new ReqParams();

        private readonly string[] _headBuildOrder = { "api", "version", "method" };

        public ReqParams ApiParams
        {
            get { return _reqData; }
        }

        public RequestBuilder()
        {
        }

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

        public string WebApi()
        {
            return String.Format("{0}{1}", "webapi", _reqData["cgiPath"] != String.Empty ? "/" + _reqData["cgiPath"] : "");
        }

        public IDictionary<string, string> CallParams
        {
            get
            {
                return _reqData
                    .Where(k => k.Key != "cgiPath")
                    .OrderBy(k => k.Key == "sid" ? -1 : _headBuildOrder.ToList().IndexOf(k.Key))
                    .ToDictionary(k => k.Key == "sid" ? "_sid" : k.Key, v => v.Value);
            }
        }

        private string _build()
        {
            var request = new StringBuilder();

            request.Append(WebApi());

            var reqHead = (from s in _headBuildOrder where _reqData[s] != String.Empty select s + "=" + System.Web.HttpUtility.UrlEncode(_reqData[s])).ToList();
            var reqParams = _params.Select(param => param.Key + "=" + System.Web.HttpUtility.UrlEncode(param.Value)).ToList();
            if (reqHead.Any() || reqParams.Any())
            {
                request.Append("?");
            }
            if (reqHead.Any())
            {
                request.Append(String.Join("&", reqHead));
            }
            if (reqParams.Any())
            {
                request.Append("&" + String.Join("&", reqParams));
            }
            if (!String.IsNullOrWhiteSpace(_reqData["sid"]))
            {
                request.Append("&_sid=" + _reqData["sid"]);
            }
            return request.ToString();
        }

    }
}
