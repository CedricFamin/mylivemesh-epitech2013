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
using client_mesh.ServiceReference;
using client_mesh.Utils;

namespace client_mesh.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        private bool _logued;
        public bool Logued
        {
            get { return _logued; }
            set { _logued = value; }
        }
        
        public MainPageViewModel()
        {
            _logued = false;
        }
    }
}
