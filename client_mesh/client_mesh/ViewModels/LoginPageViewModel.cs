using System;
using System.Windows.Input;
using client_mesh.ServiceReference;
using client_mesh.Utils;

namespace client_mesh.ViewModels
{
	public class LoginPageViewModel : BindableObject
	{

        static User g_User = null;

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

        private string _password;
        public string Password
        {
            get { return _password; }
            set { 
                _password = value;
                RaisePropertyChange("Password");
            }
        }

        private bool _logued;
        public bool Logued
        {
            get { return _logued; }
            set { 
                _logued = value;
                RaisePropertyChange("Logued");
            }
        }
        
        

        private WebResult.ErrorCodeList _error;
        public WebResult.ErrorCodeList ErrorCode
        {
            get { return _error; }
            set { 
                _error = value;
                RaisePropertyChange("ErrorCode");
            }
        }
        
        public LoginPageViewModel()
        {
            Login = new RelayCommand((param) => LoginBody(param as string[]));
            accountService = new AccountClient();
            accountService.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(OnEndLogin);
            accountService.RegisterCompleted += new EventHandler<RegisterCompletedEventArgs>(OnEndRegister);
            _error = 0;
            _logued = false;
        }
        
        private void OnEndLogin(object sender, LoginCompletedEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.Result.ErrorCode == WebResult.ErrorCodeList.SUCCESS)
                {
                    Username = args.Result.Value.username;
                    Logued = true;
                    g_User = args.Result.Value;
                }
                ErrorCode = args.Result.ErrorCode;
            }
        }

        private void OnEndRegister(object sender, RegisterCompletedEventArgs args)
        {
            Username = "Enregistrement";
        }

        private void LoginBody(string[] param)
        {
            //accountService.RegisterAsync("test", "test", "test");
            accountService.LoginAsync(Username, Password);
        }
	}
}