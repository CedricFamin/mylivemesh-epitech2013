using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;
using client_mesh.Utils;
using System.Windows.Controls;
using client_mesh.ServiceReference;

namespace client_mesh.ViewModels
{
	public class DashboardModel : BindableObject
    {
        #region Properties
        public ICommand OpenProfileWindow { get; private set; }
        private ViewModelLocator Locator { get; set; }

        private bool _profileFormOpen;
        public bool ProfileFormOpen
        {
            get { return _profileFormOpen; }
            set { 
                _profileFormOpen = value;
                RaisePropertyChange("ProfileFormOpen");
            }
        }

        public User User
        {
            get
            {
                return Locator.LoginPageViewModel.User;
            }
        }
        #endregion

        #region CTOR
        public DashboardModel()
		{
			OpenProfileWindow = new RelayCommand((param) => this.OpenProfileWindowBody());
            Locator = new ViewModelLocator();
            Locator.LoginPageViewModel.PropertyChanged += new PropertyChangedEventHandler(LoginPageViewModel_PropertyChanged);
		}

        void LoginPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "User")
                RaisePropertyChange("User");
        }
        #endregion

        #region Command Body
        private void OpenProfileWindowBody()
        {
            ProfileFormOpen = !ProfileFormOpen;
        }
        #endregion
    }
}