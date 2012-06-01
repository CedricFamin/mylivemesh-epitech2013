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
using System.IO;
using System.Windows.Threading;
using client_mesh.ViewModels;
using Liquid.Components;

namespace client_mesh.Utils
{
    public class ProgressBarDataStreamBehavior : Behavior<ProgressBar>
    {
        private DispatcherTimer _clockTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 10) };
        public Stream Stream
        {
            get { return (Stream)GetValue(StreamProperty); }
            set { SetValue(StreamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stream.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StreamProperty =
            DependencyProperty.Register("Stream", typeof(Stream), typeof(ProgressBarDataStreamBehavior), null);

        protected override void OnAttached()
        {
            base.OnAttached();
            _clockTimer.Tick += new EventHandler(_clockTimer_Tick);
            _clockTimer.Start();
        }

        protected override void OnDetaching()
        {
            _clockTimer.Stop();
        }


        void _clockTimer_Tick(object sender, EventArgs e)
        {
            if (Stream != null && Stream.CanRead)
            {
                AssociatedObject.Maximum = Stream.Length;
                AssociatedObject.Value = Stream.Position;
            }
        }

        
    }
}
