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

    class TaskDeleteOptions
    {
        [OptionArray("id", Required = true, DefaultValue = new string[] { }, HelpText = "Space-separated list of task ids to delete")]
        public string[] Id { get; set; }

        [OptionArray('f', "force", DefaultValue = false, HelpText = "Delete tasks and force to move uncompleted download files to the destination")]
        public bool Force { get; set; }
    }

    class TaskPauseOptions
    {
        [OptionArray("id", Required = true, DefaultValue = new string[] { }, HelpText = "Space-separated list of task ids to pause")]
        public string[] Id { get; set; }
    }

    class TaskResumeOptions
    {
        [OptionArray("id", Required = true, DefaultValue = new string[] { }, HelpText = "Space-separated list of task ids to resume")]
        public string[] Id { get; set; }
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

    class NewOptions
    {
        [Option('f', "file", Required = false, HelpText = "Filename to upload (e.g. .torrent)")]
        public string Filename { get; set; }

        [Option('u', "url", Required = false, HelpText = "URL of download task")]
        public string Uri { get; set; }
    }

    class Options
    {
        [VerbOption("list", HelpText = "List Download Station's tasks")]
        public ListOptions ListVerb { get; set; }

        [VerbOption("task", HelpText = "Get specified tasks with info")]
        public TaskOptions TaskVerb { get; set; }

        [VerbOption("new", HelpText = "Create new download task")]
        public NewOptions NewVerb { get; set; }

        [VerbOption("delete", HelpText = "Delete download task(s)")]
        public TaskDeleteOptions DeleteVerb { get; set; }

        [VerbOption("pause", HelpText = "Pause download task(s)")]
        public TaskPauseOptions PauseVerb { get; set; }

        [VerbOption("resume", HelpText = "Resume download task(s)")]
        public TaskResumeOptions ResumeVerb { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("SynoSharp CLI / Under construction");
            return usage.ToString();
        }
    }

}
