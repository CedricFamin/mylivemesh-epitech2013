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
using System.Collections.Generic;

namespace client_mesh.Utils
{
    public class BringToFrontBehavior : Behavior<UserControl>
    {
        private Canvas _canvas =  null;
        private Canvas CanvasParent { 
            get {
                if (_canvas == null)
                {
                    DependencyObject parent = AssociatedObject;
                    while (parent.GetType() != typeof(Canvas))
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                    }
                    _canvas = parent as Canvas;
                }
                return _canvas;
            } 
        }

        static private int ZMin { get; set; }
        static private int ZMax { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
        }

        void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.SetZIndex(AssociatedObject, ++ZMax);
        }
    }
}
