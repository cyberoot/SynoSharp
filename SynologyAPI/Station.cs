using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SynologyRestDAL;

namespace SynologyAPI
{
    public class Station
    {
        protected Uri BaseUrl;
        protected string Username;
        protected string Password;
        protected string Sid;
        protected string InternalSession;
        protected WebProxy Proxy;
        protected ApiInfo ApiInfo;

        protected virtual string[] ImplementedApi
        {
            get
            {
                return new[] { "SYNO.API.Auth" };
            }
        }

        public Station()
        {
            /*
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true; // **** Ignore any ssl errors
                };
             */
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
        }
      
        public Station(Uri url, string username, string password)
            : this()
        {
            BaseUrl = url;
            Username = username;
            Password = password;
            InternalSession = "DownloadStation";
        }

        public Station(Uri url, string username, string password, WebProxy proxy)
            : this(url, username, password)
        {
            if (proxy != null)
            {
                Proxy = proxy;
            }
        }

        protected string _run(RequestBuilder requestBuilder)
        {
            var request = WebRequest.Create(BaseUrl.ToString() + requestBuilder);
            if (Proxy != null)
            {
                request.Proxy = Proxy;
            }
            WebResponse response = request.GetResponse();
            string resJson;
            using (var reader = new StreamReader(stream: response.GetResponseStream()))
            {
                resJson = reader.ReadToEnd();
            }
            return resJson;
        }

        public RequestBuilder CreateRequest(KeyValuePair<string, ApiSpec> apiSpec)
        {
            return (new RequestBuilder()).
                        Api(apiSpec.Key).
                        CgiPath(apiSpec.Value.Path).
                        Version(apiSpec.Value.MaxVersion.ToString()).
                        Method("");
        }

        public RequestBuilder CreateRequest(RequestBuilder requestBuilder, KeyValuePair<string, ApiSpec> apiSpec)
        {
            return requestBuilder.
                        Api(apiSpec.Key).
                        CgiPath(apiSpec.Value.Path).
                        Version(apiSpec.Value.MaxVersion.ToString());
        }

        public T PostFile<T>(string apiName, string method, string fileName, Stream fileStream, string fileParam = "file")
        {
            var stationApi = GetApi(apiName).FirstOrDefault();
            return stationApi.Key == null ? default(T) : 
                JsonHelper.FromJson<T>(
                        _postFile(
                            CreateRequest((new RequestBuilder(Sid)).Session(Sid).Method(method), stationApi),
                            fileName,
                            fileStream,
                            fileParam
                       )
            );
        }

        public T Call<T>(string apiName, RequestBuilder requestBuilder)
        {
            var stationApi = GetApi(apiName).FirstOrDefault();
            return stationApi.Key == null ? default(T) : JsonHelper.FromJson<T>(_run(CreateRequest(requestBuilder, stationApi)));
        }

        protected T CallMethod<T>(string apiName, string method)
        {
            return Call<T>(apiName, (new RequestBuilder(Sid)).Session(Sid).Method(method));
        }

        public T CallMethod<T>(string apiName, string method, ReqParams param)
        {
            return Call<T>(apiName, (new RequestBuilder(Sid)).Method(method, param));
        }

        public Dictionary<string,ApiSpec> GetApi(string apiName)
        {
            if (ApiInfo == null)
            {
                ApiInfo = JsonHelper.FromJson<ApiInfo>(_run(
                    (new RequestBuilder()).
                        Api("SYNO.API.Info").
                        AddParam("query", String.Join(",", ImplementedApi))
                   )
                );
            }
            return ApiInfo.Data.Where(p => p.Key.StartsWith(apiName)).ToDictionary(t => t.Key, t => t.Value);
        }

        public bool Login()
        {
            var loginResult = CallMethod<LoginResult>("SYNO.API.Auth",
                "login", new ReqParams()
                        {
                            {"account", Username},
                            {"passwd", Password},
                            {"session", InternalSession},
                            {"format", "sid"}
                        }
            );
            if (loginResult.Success)
            {
                Sid = loginResult.Data.Sid;
            }
            return loginResult.Success;
        }

        public bool Logout()
        {
            var logoutResult = CallMethod<TResult<Object>>("SYNO.API.Auth", "logout", new ReqParams() { {"session", InternalSession} });
            return logoutResult.Success;
        }

        protected string _postFile(RequestBuilder requestBuilder, string fileName, Stream fileStream, string fileParam = "file")
        {
            HttpContent fileStreamContent = new StreamContent(fileStream);
            var requestHandler = new HttpClientHandler();
            if (Proxy != null)
            {
                requestHandler.Proxy = Proxy;
            }
            string requestUri = BaseUrl.ToString() + requestBuilder;
            System.IO.Stream result = null;
            string resJson = String.Empty;
            using (var client = new HttpClient(requestHandler))
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent, fileParam, fileName);
                    var response = client.PostAsync(requestUri, formData).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStreamAsync().Result;
                    }
                }
            }
            using (var reader = new StreamReader(stream: result))
            {
                resJson = reader.ReadToEnd();
            }
            return resJson;
        }

    }
}
