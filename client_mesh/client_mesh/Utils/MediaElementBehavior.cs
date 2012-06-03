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
using System.Threading;
using System.Windows.Threading;
using Liquid;

namespace client_mesh.Utils
{
    public abstract class MediaCommandBehavior : Behavior<FrameworkElement>
    {
        public MediaElement Media
        {
            get { return (MediaElement)GetValue(MediaProperty); }
            set { SetValue(MediaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Media.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MediaProperty =
            DependencyProperty.Register("Media", typeof(MediaElement), typeof(MediaCommandBehavior), null);

        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Click();
        }

        protected abstract void Click();
    }

    public class PlayCommandBehavior : MediaCommandBehavior
    {
        protected override void Click()
        {
            if (Media != null)
                Media.Play();
        }
    }

    public class PauseCommandBehavior : MediaCommandBehavior
    {
        protected override void Click()
        {
            if (Media != null)
                Media.Pause();
        }
    }

    public class StopCommandBehavior : MediaCommandBehavior
    {
        protected override void Click()
        {
            if (Media != null)
                Media.Stop();
        }
    }

    public class BeginCommandBehavior : MediaCommandBehavior
    {
        protected override void Click()
        {
            if (Media != null)
            {
                Media.Position = new TimeSpan();
            }
        }
    }

    public class SliderCommandBehavior : Behavior<Slider>
    {
        private DispatcherTimer _clockTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 10) };
        public MediaElement Media
        {
            get { return (MediaElement)GetValue(MediaProperty); }
            set { SetValue(MediaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Media.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MediaProperty =
            DependencyProperty.Register("Media", typeof(MediaElement), typeof(SliderCommandBehavior), null);

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown), true);
            AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            AssociatedObject.IsEnabled = true;
            _clockTimer.Tick += new EventHandler(_clockTimer_Tick);
            _clockTimer.Start();
        }

        void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _clockTimer.Stop();
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Media != null)
            {
                Media.Position = new TimeSpan(0, 0, 0, 0, (int)AssociatedObject.Value);
            }
            _clockTimer.Start();
        }

        protected override void OnDetaching()
        {
            _clockTimer.Stop();
        }

        void _clockTimer_Tick(object sender, EventArgs e)
        {
            if (Media != null)
            {
                AssociatedObject.Value = Media.Position.TotalMilliseconds;
                AssociatedObject.Maximum = Media.NaturalDuration.TimeSpan.TotalMilliseconds;
                AssociatedObject.LargeChange = AssociatedObject.Maximum;
            }
        }
    }
}
