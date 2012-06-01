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
using System.Collections.Generic;

namespace client_mesh.ViewModels
{
    public class ViewModelLocator
    {
        static private LoginPageViewModel _loginPageViewModel = new LoginPageViewModel();
        public LoginPageViewModel LoginPageViewModel {
            get {
                return _loginPageViewModel;
            }
        }

        static private DashboardModel _dashboardModel = new DashboardModel();
        public DashboardModel DashboardModel {
            get {
                return _dashboardModel;
            }
        }

        static private EditProfileViewModel _editProfileViewModel = new EditProfileViewModel();
        public EditProfileViewModel EditProfileViewModel
        {
            get
            {
                return _editProfileViewModel;
            }
        }

        static private LoggerViewModel _loggerViewModel = new LoggerViewModel();
        public LoggerViewModel LoggerViewModel
        {
            get
            {
                return _loggerViewModel;
            }
        }

        static private FolderViewModel _folderViewModel = new FolderViewModel();
        public FolderViewModel FolderViewModel
        {
            get
            {
                return _folderViewModel;
            }
        }

        static private FileUploaderViewControl _FileUploaderViewModel = new FileUploaderViewControl();
        public FileUploaderViewControl FileUploaderViewModel { get { return _FileUploaderViewModel; } }

        static private List<FolderDefinition> _folders = new List<FolderDefinition>();
        public List<FolderDefinition> Folders { get { return _folders; } }

        public ViewModelLocator()
        {

        }
    }
}
