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
using Liquid;

namespace client_mesh.Utils
{
    public class RemoveWindowFromParentBehavior : Behavior<Dialog>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.CloseCompleted += new DialogEventHandler(AssociatedObject_CloseCompleted);
        }

        void AssociatedObject_CloseCompleted(object sender, DialogEventArgs e)
        {
            DependencyObject parent = AssociatedObject.Parent;
            while (parent.GetType().IsInstanceOfType(typeof(UserControl)))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            UserControl us = parent as UserControl;
            parent = VisualTreeHelper.GetParent(parent);
            while (parent.GetType().IsInstanceOfType(typeof(Panel)))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            Panel panel = parent as Panel;
            if (panel != null)
            {
                panel.Children.Remove(us);
            }
        }
    }
}
