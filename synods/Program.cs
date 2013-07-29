using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SynologyAPI;
using SynologyRestDAL;
using StdUtils;
using CommandLine;
using System.Collections.Specialized;
using System.Configuration;

namespace synods
{
    class Program
    {
        static void Main(string[] args)
        {
            string invokedVerb = "";
            object invokedVerbInstance = null;
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options,
                                        (verb, subOptions) =>
                                        {
                                            invokedVerb = verb;
                                            invokedVerbInstance = subOptions;
                                        }))
            {
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            var ds = new DownloadStation(new Uri(appSettings["host"]), appSettings["username"], appSettings["password"], CreateProxy(appSettings["proxy"]));
            switch(invokedVerb)
            {
                case("list"):
                    var listOptions = (ListOptions)invokedVerbInstance;
                    if (ds.Login())
                    {
                        var listResult = ds.List(String.Join(",", listOptions.Details));
                        if (listResult.Success)
                        {
                            var taskList = from task in listResult.Data.Tasks select task;
                            if (listOptions.Status.Count() > 0)
                            {
                                var statusesToList = new List<string>(listOptions.Status);
                                taskList = from task in taskList where statusesToList.Contains(task.Status) select task;
                            }
                            foreach (var task in taskList)
                            {
                                Console.WriteLine(ObjectUtils.HumanReadable(task));
                                Console.WriteLine();
                            }
                        }
                        ds.Logout();
                    }
                    break;

                case ("task"):
                    var taskOptions = (TaskOptions)invokedVerbInstance;
                    if (ds.Login())
                    {
                        var taskResult = ds.GetTasks(taskOptions.Id, taskOptions.Details);
                        if (taskResult.Success)
                        {
                            foreach (var task in taskResult.Data.Tasks)
                            {
                                Console.WriteLine(ObjectUtils.HumanReadable(task));
                                Console.WriteLine();
                            }
                        }
                        ds.Logout();
                    }
                    break;
                default:
                    break;
            }

            Console.ReadLine();
        }


        public static WebProxy CreateProxy(string proxyUrl)
        {
            if (String.IsNullOrWhiteSpace(proxyUrl))
            {
                return null;
            }
            return new WebProxy(new Uri(proxyUrl));
        }
    }
}
