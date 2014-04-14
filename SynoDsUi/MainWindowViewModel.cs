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
using AutoMapper;
using SynoDsUi.Annotations;
using SynologyAPI;
using SynologyRestDAL.Ds;

namespace SynoDsUi
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private DownloadStation DownloadStation;

        public ObservableCollection<TaskViewModel> _tasks;

        public ObservableCollection<TaskViewModel> Tasks { get { return _tasks; } }

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
                    var taskList = from task in listResult.Data.Tasks select Mapper.Map<TaskViewModel>(task);
                    _tasks = new ObservableCollection<TaskViewModel>(taskList);
                }
                OnPropertyChanged("Tasks");
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
