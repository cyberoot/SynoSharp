using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;

namespace SynologyApiTest
{
    using NUnit.Framework;
    public abstract class BaseSynologyTests
    {
        public string Host { get { return GetAppSetting("host"); } }
        public string Username { get { return GetAppSetting("username"); } }
        public string Password { get { return GetAppSetting("password"); } }
        public string Proxy { get { return GetAppSetting("proxy"); } }

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