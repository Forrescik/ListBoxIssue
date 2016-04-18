using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ListBoxIssue
{
    public class BorderBehavior : Behavior<Border>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Initialized += AssociatedObjectOnInitialized;
            AssociatedObject.IsVisibleChanged += AssociatedObjectOnIsVisibleChanged;
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            AssociatedObject.RequestBringIntoView += AssociatedObjectOnRequestBringIntoView;
            base.OnAttached();
        }

        private static void AssociatedObjectOnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs requestBringIntoViewEventArgs)
        {
            

        }

        private static void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            
        }

        private static void AssociatedObjectOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            
        }

        private static void AssociatedObjectOnInitialized(object sender, EventArgs eventArgs)
        {
            
        }
    }
}
