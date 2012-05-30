using System;
using System.Windows.Input;
using client_mesh.ServiceReference;
using client_mesh.Utils;

namespace client_mesh.ViewModels
{
	public class LoginPageViewModel : BindableObject
	{

		public ICommand Login { get; private set; }
        private AccountClient accountService { get; set; }
        private string _username;

        public string Username
        {
            get { return _username; }
            set { 
                _username = value;
                RaisePropertyChange("Username");
            }
        }
        
        public LoginPageViewModel()
        {
            Login = new RelayCommand((param) => LoginBody(param as string[]));
            accountService = new AccountClient();
            accountService.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(OnEndLogin);
            accountService.RegisterCompleted += new EventHandler<RegisterCompletedEventArgs>(OnEndRegister);
        }
        
        private void OnEndLogin(object sender, LoginCompletedEventArgs args)
        {
            Username = args.Result.Value.username;
        }

        private void OnEndRegister(object sender, RegisterCompletedEventArgs args)
        {
            Username = "Enregistrement";
        }

        private void LoginBody(string[] param)
        {
            //accountService.RegisterAsync("test", "test", "test");
            accountService.LoginAsync("test", "test");
        }
	}
}