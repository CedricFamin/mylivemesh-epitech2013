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
using System.Windows.Interactivity;

namespace client_mesh.Utils
{
    public class OpenFolderWIndowBehavior : Behavior<MenuItem>
    {
        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register("DataContext", typeof(object), typeof(OpenFolderWIndowBehavior), null);



        public Panel Receiver
        {
            get { return (Panel)GetValue(ReceiverProperty); }
            set { SetValue(ReceiverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Receiver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReceiverProperty =
            DependencyProperty.Register("Receiver", typeof(Panel), typeof(OpenFolderWIndowBehavior), null);

        
        
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += new RoutedEventHandler(AssociatedObject_Click);
            
        }

        void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderControl win = new OpenFolderControl()
            {
                DataContext = DataContext,
            };
            
            BringToFrontBehavior bh = new BringToFrontBehavior();
            Interaction.GetBehaviors(win).Add(bh);
            DependencyObject parent = Receiver;
    
            while (parent.GetType() != typeof(FolderWindowControl))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            FolderWindowControl canvas = parent as FolderWindowControl;
            
            if (canvas != null)
            {
                canvas.Parent.Children.Add(win);
                win.Container = canvas.Parent;
            }
        }
    }
}
