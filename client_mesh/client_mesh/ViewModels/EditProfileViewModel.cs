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
    public class EditProfileViewModel : BindableObject
    {
        #region Properties
        private ViewModelLocator VMLocator = new ViewModelLocator();
        private AccountClient _accountService = new AccountClient();

        public User User
        {
            get {
                return VMLocator.LoginPageViewModel.User;
            }
        }

        public ICommand UpdateUser { get; private set; }
        #endregion

        #region CTor
        public EditProfileViewModel()
        {
            UpdateUser = new RelayCommand((param) => UpdateUserBody());
            _accountService.UpdateCompleted += new EventHandler<UpdateCompletedEventArgs>(_accountService_UpdateCompleted);
            VMLocator.LoginPageViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(LoginPageViewModel_PropertyChanged);
        }
        #endregion

        #region Event Handler
        void LoginPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "User")
                RaisePropertyChange("User");
        }

        private void _accountService_UpdateCompleted(object sender, UpdateCompletedEventArgs e)
        {
            if (e.Error == null)
                VMLocator.LoggerViewModel.AddLog("Update user information: " + e.Result.ErrorCode);
        }
        #endregion

        #region Command body
        private void UpdateUserBody()
        {
            _accountService.UpdateAsync(User);
        }
        #endregion
    }
}
