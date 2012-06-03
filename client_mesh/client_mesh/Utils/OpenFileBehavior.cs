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
using client_mesh.ServiceReference1;

namespace client_mesh.Utils
{
    public class OpenFileBehavior : Behavior<FrameworkElement>
    {
        public DependencyObject Receiver
        {
            get { return (Panel)GetValue(ReceiverProperty); }
            set { SetValue(ReceiverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Receiver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReceiverProperty =
            DependencyProperty.Register("Receiver", typeof(DependencyObject), typeof(OpenFileBehavior), null);

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FileDefinition file = AssociatedObject.DataContext as FileDefinition;
            FrameworkElement win = null;
            if (file == null)
                return;
            if (file.MimeType.Contains("image"))
            {
                ImageWindowControl imageWin = new ImageWindowControl();

                win = imageWin;
            }
            else if (file.MimeType.Contains("video"))
            {
                VideoWindowControl lwin = new VideoWindowControl();
                win = lwin;
            }
            else if (file.MimeType.Contains("audio"))
            {
                VideoWindowControl lwin = new VideoWindowControl();
                win = lwin;
                
            }
            else if (file.MimeType.Contains("text"))
            {
                win = new TextWindowControl();
            }
            if (win != null)
            {
                win.DataContext = AssociatedObject.DataContext;
                BringToFrontBehavior bh = new BringToFrontBehavior();
                Interaction.GetBehaviors(win).Add(bh);
                DependencyObject parent = Receiver;
                while (parent.GetType() != typeof(OpenFolderControl))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                OpenFolderControl canvas = parent as OpenFolderControl;

                if (canvas != null)
                {
                    canvas.Container.Children.Add(win);
                }
            }

        }
    }
}
