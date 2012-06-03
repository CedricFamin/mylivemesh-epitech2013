using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using client_mesh.Utils;

namespace client_mesh
{
    public partial class OpenFolderControl : UserControl
	{
        public Panel Container
        {
            get { return (Panel)GetValue(ContainerProperty); }
            set { SetValue(ContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Container.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContainerProperty =
            DependencyProperty.Register("Container", typeof(Panel), typeof(OpenFolderControl), null);

		public OpenFolderControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}
	}
}