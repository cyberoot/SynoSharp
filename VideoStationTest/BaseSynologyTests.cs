using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;

namespace SynologyApiTest
{
    public abstract class BaseSynologyTests
    {
        public string Host => GetAppSetting("host");
        public string Username => GetAppSetting("username");
        public string Password => GetAppSetting("password");
        public string Proxy => GetAppSetting("proxy");

        protected static WebProxy CreateProxy(string proxyUrl)
        {
            return String.IsNullOrWhiteSpace(proxyUrl) ? null : new WebProxy(new Uri(proxyUrl));
        }

        protected static string GetAppSetting(string host)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[host];
        }
    }
}