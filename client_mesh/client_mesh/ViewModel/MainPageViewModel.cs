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

namespace client_mesh.ViewModel
{
    public class MainPageViewModel : BindableObject
    {
        public ICommand Login { get; private set; }
        private ServiceReference.AccountClient accountService { get; set; }
        private string _username;

        public string Username
        {
            get { return _username; }
            set { 
                _username = value;
                RaisePropertyChange("Username");
            }
        }

        public MainPageViewModel()
        {
            Login = new RelayCommand((param) => LoginBody(param as string[]));
            accountService = new ServiceReference.AccountClient();
            accountService.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(OnEndLogin);
            
        }

        private void OnEndLogin(object sender, LoginCompletedEventArgs args)
        {
            Username = args.Result.Value.username;
        }

        private void LoginBody(string[] param)
        {
            accountService.LoginAsync("Mediagora", "toto");
        }
    }
}
