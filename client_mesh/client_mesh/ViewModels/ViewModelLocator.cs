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

namespace client_mesh.ViewModels
{
    public class ViewModelLocator
    {
        private LoginPageViewModel _loginPageViewModel = null;

        public LoginPageViewModel LoginPageViewModel
        {
            get {
                if (_loginPageViewModel == null)
                    _loginPageViewModel = new LoginPageViewModel();
                return _loginPageViewModel;
            }
        }

        private DashboardModel _dashboardModel = null;

        public DashboardModel DashboardModel
        {
            get
            {
                if (_dashboardModel == null)
                    _dashboardModel = new DashboardModel();
                return _dashboardModel;
            }
        }
        
        public ViewModelLocator()
        {

        }
    }
}
