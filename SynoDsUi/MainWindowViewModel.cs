using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using AutoMapper;
using SynoDsUi.Annotations;
using SynologyAPI;
using SynologyRestDAL.Ds;

namespace SynoDsUi
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private DownloadStation DownloadStation;

        private TaskStatusViewModel CurrentStatus = new TaskStatusViewModel("all");

        public ObservableCollection<TaskViewModel> _allTasks;

        private IDictionary<string, ObservableCollection<TaskViewModel>> _tasksByStatus;

        public ObservableCollection<TaskViewModel> CurrentTasks { get { return _tasksByStatus[CurrentStatus.Title]; } }

        private ObservableCollection<TaskStatusViewModel> _statuses;

        public ObservableCollection<TaskStatusViewModel> Statuses { get { return _statuses; } }

        public TaskStatusViewModel CurrentStatusTab
        {
            get { return CurrentStatus; }
            set { CurrentStatus = value; OnPropertyChanged("CurrentTasks"); }
        }

        public MainWindowViewModel()
        {
            Mapper.CreateMap<Task, TaskViewModel>();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            DownloadStation = new DownloadStation(new Uri(appSettings["host"]), appSettings["username"], appSettings["password"], CreateProxy(appSettings["proxy"]));

            if (DownloadStation.Login())
            {
                var listResult = DownloadStation.List(String.Join(",", new []{ "detail", "transfer", "file", "tracker" }));
                if (listResult.Success)
                {
                    var taskList = (from task in listResult.Data.Tasks orderby task.Additional.Detail.CreateTime select Mapper.Map<TaskViewModel>(task)).ToList();
                    _allTasks = new ObservableCollection<TaskViewModel>(taskList);
                    var statusList = (new List<TaskStatusViewModel>() {new TaskStatusViewModel("all")}).Concat(
                        taskList.Select(t => t.Status).Distinct().OrderBy(s => s).Select(s => new TaskStatusViewModel(s)));
                    _statuses = new ObservableCollection<TaskStatusViewModel>(statusList);
                    _tasksByStatus = new Dictionary<string, ObservableCollection<TaskViewModel>>();
                    foreach (var taskStatus in _statuses)
                    {
                        if (taskStatus.Title == "all")
                        {
                            _tasksByStatus.Add(taskStatus.Title, _allTasks);
                            continue;
                        }
                        var tasks = new ObservableCollection<TaskViewModel>(_allTasks.Where(t => t.Status == taskStatus.Title).OrderBy(t => t.Additional.Detail.CreateTime));
                        _tasksByStatus.Add(taskStatus.Title, tasks);
                    }

                }
                OnPropertyChanged("CurrentTasks");
                OnPropertyChanged("Statuses");
                
                DownloadStation.Logout();

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static WebProxy CreateProxy(string proxyUrl)
        {
            if (String.IsNullOrWhiteSpace(proxyUrl))
            {
                return null;
            }
            return new WebProxy(new Uri(proxyUrl));
        }
    }
}
