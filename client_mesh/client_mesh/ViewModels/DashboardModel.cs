using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;
using client_mesh.Utils;
using System.Windows.Controls;

namespace client_mesh.ViewModels
{
	public class DashboardModel : BindableObject
	{
        public ICommand OpenProfileWindow { get; private set; }
		public DashboardModel()
		{
			OpenProfileWindow = new RelayCommand((param) => this.OpenProfileWindowBody());
		}

        private void OpenProfileWindowBody()
        {  
        }

	}
}