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
using System.IO;

namespace client_mesh.Utils
{
    public class DownloadTextBehavior : Behavior<TextBlock>
    {
        WebClient wc = new WebClient();

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
            
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            FileDefinition file = AssociatedObject.DataContext as FileDefinition;
            if (file != null)
            {
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
                wc.DownloadStringAsync(new Uri(file.FileUri));
            }
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled == false && e.Error == null)
                AssociatedObject.Text = e.Result;
        }


    }
}
