using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynoDsUi
{
    public class TaskStatusViewModel
    {
        private string _title;
        public TaskStatusViewModel(string title)
        {
            _title = title;
        }

        public string Title { get { return _title; } }
    }
}
