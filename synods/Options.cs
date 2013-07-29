using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace synods
{
    class ListOptions
    {
        [OptionArray('s', "status", DefaultValue = new string[] { }, HelpText = "Space-separated list of statuses to include when listing tasks (waiting, downloading, paused, finishing, finished, hash_checking, seeding, filehost_waiting, extracting, error)")]
        public string[] Status { get; set; }

        [OptionArray('a', "add", DefaultValue = new string[] { }, HelpText = "Space-separated list of deatils to include when listing tasks (detail, transfer, file, tracker, peer)")]
        public string[] Details { get; set; }
    }

    class TaskOptions
    {
        [OptionArray("id", Required = true, DefaultValue = new string[] { }, HelpText = "Space-separated list of task ids to get info about")]
        public string[] Id { get; set; }

        [OptionArray('a', "add", DefaultValue = new string[] { }, HelpText = "Space-separated list of deatils to include when getting task info (detail, transfer, file, tracker, peer)")]
        public string[] Details { get; set; }
    }

    class InfoOptions
    {
        [Option("get", DefaultValue = true, HelpText = "Get Download Station version info and authenticated role")]
        public bool Info { get; set; }
    }

    class ConfigOptions
    {
        [Option("get", DefaultValue = true, HelpText = "Get Download Station config")]
        public bool Config { get; set; }
    }

    class Options
    {
        [VerbOption("list", HelpText = "List Download Station's tasks")]
        public ListOptions ListVerb { get; set; }

        [VerbOption("task", HelpText = "Get specified tasks with info")]
        public TaskOptions TaskVerb { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("SynoSharp CLI / Under construction");
            return usage.ToString();
        }
    }

}
