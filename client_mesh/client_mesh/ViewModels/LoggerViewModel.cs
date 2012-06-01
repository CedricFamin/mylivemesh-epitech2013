using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace client_mesh.ViewModels
{
    public class LoggerViewModel : BindableObject
    {
        private ObservableCollection<string> _log;

        public ObservableCollection<string> Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public LoggerViewModel()
        {
            _log = new ObservableCollection<string>();
        }

        public void AddLog(string s)
        {
            Log.Add(DateTime.Now.ToString("t") + " -> " + s);
            RaisePropertyChange("Log");
        }
        
    }
}
