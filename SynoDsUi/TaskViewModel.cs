using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SynologyRestDAL.Ds;

namespace SynoDsUi
{
    public class TaskViewModel : Task
    {
        public string Filename { get { return Title; } }

        public double Progress { get { return long.Parse(Size) == 0 ? 0 : (long.Parse(Additional.Transfer.SizeDownloaded) / long.Parse(Size)) * 100; } }

        public string DownloadedLabel { get { return string.Format("{0:F} Mb", long.Parse(Additional.Transfer.SizeDownloaded) / 1024 / 1024); } }
        
        public string UploadedLabel { get { return string.Format("{0:F} Mb", long.Parse(Additional.Transfer.SizeUploaded) / 1024 / 1024); } }

        public string SizeLabel { get { return string.Format("{0:F} Mb", long.Parse(Size) / 1024 / 1024); } }

        public string CreatedLabel { get { return Additional.Detail.CreateTime == null ? "" : String.Format("{0:U}", DateTime.Parse (Additional.Detail.CreateTime)); } }
    }
}
