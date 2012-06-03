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
using System.Collections.ObjectModel;
using client_mesh.Utils;

namespace client_mesh.ViewModels
{
    public class UserManagementViewModel : BindableObject
    {
        AccountClient _accountService = new AccountClient();
        ViewModelLocator _locator = new ViewModelLocator();

        #region Ppties
        public ICommand CreateUser { get; private set; }
        public ICommand DeleteUser { get; private set; }
        public ICommand UpdateUser { get; private set; }
        public ICommand RefresUsers { get; private set; }

        public User SelectedUser { get; set; }
        public User NewUser { get; set; }
        public ObservableCollection<User> Users { get; set; }
        #endregion

        #region Ctor
        public UserManagementViewModel()
        {
            _accountService.RegisterCompleted += new EventHandler<RegisterCompletedEventArgs>(_accountService_RegisterCompleted);
            _accountService.DeleteCompleted += new EventHandler<DeleteCompletedEventArgs>(_accountService_DeleteCompleted);
            _accountService.UpdateCompleted += new EventHandler<UpdateCompletedEventArgs>(_accountService_UpdateCompleted);
            _accountService.UserListCompleted += new EventHandler<UserListCompletedEventArgs>(_accountService_UserListCompleted);

            CreateUser = new RelayCommand((param) => CreateUserBody());
            DeleteUser = new RelayCommand((param) => DeleteUserBody(param as User));
            UpdateUser = new RelayCommand((param) => UpdateUserBody(param as User));
            RefresUsers = new RelayCommand((param) => RefresUsersBody());
            NewUser = new User();
            Users = new ObservableCollection<User>();
        }
        #endregion

        #region Event handler
        void _accountService_UpdateCompleted(object sender, UpdateCompletedEventArgs e)
        {
            if (e.Error == null)
                _locator.LoggerViewModel.AddLog("Update user:" + e.Result.ErrorCode);
        }

        void _accountService_DeleteCompleted(object sender, DeleteCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                _locator.LoggerViewModel.AddLog("Delete user:" + e.Result.ErrorCode);
                RefresUsersBody();
            }
        }

        void _accountService_RegisterCompleted(object sender, RegisterCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                _locator.LoggerViewModel.AddLog("Add new user:" + e.Result.ErrorCode);
                NewUser.username = "";
                NewUser.password = "";
                NewUser.email   = "";
                RaisePropertyChange("NewUser");
                RefresUsersBody();
            }
        }

        void _accountService_UserListCompleted(object sender, UserListCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ErrorCode == WebResult.ErrorCodeList.SUCCESS)
                {
                    Users = e.Result.Value;
                    RaisePropertyChange("Users");
                }
                _locator.LoggerViewModel.AddLog("Get user list: " + e.Result.ErrorCode);
            }
        }
        #endregion

        #region Command Body
        private void UpdateUserBody(User user)
        {
            if (user != null)
                _accountService.UpdateAsync(user);
        }

        private void DeleteUserBody(User user)
        {
            if (user != null)
                _accountService.DeleteAsync(user.id);
        }

        private void CreateUserBody()
        {
            _accountService.RegisterAsync(NewUser.username, NewUser.email, NewUser.password);
        }

        private void RefresUsersBody()
        {
            _accountService.UserListAsync();
        }
        #endregion
    }
}
