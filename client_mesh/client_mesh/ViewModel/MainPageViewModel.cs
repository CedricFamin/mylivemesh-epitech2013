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
        private ServiceReference.Account accountService { get; set; }

        public MainPageViewModel()
        {
            Login = new RelayCommand((param) => LoginBody(param as string[]));
            accountService = new ServiceReference.AccountClient();
        }

        void OnEndLoginBody(IAsyncResult result)
        {
            int i = 1;
        }

        private void LoginBody(string[] param)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            int test = ((User)(null)).GetHashCode();
            accountService.BeginLogin("Mediagora", "test", new AsyncCallback(OnEndLoginBody), null);
        }
    }
}
