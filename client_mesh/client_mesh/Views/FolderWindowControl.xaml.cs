using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace client_mesh
{
	public partial class FolderWindowControl : UserControl
	{
        public Panel Parent
        {
            get { return (Panel)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Parent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.Register("Parent", typeof(Panel), typeof(FolderWindowControl), null);

        
		public FolderWindowControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}
	}
}