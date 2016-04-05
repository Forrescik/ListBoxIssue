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

        private void AssociatedObjectOnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs requestBringIntoViewEventArgs)
        {
            

        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            
        }

        private void AssociatedObjectOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            
        }

        private void AssociatedObjectOnInitialized(object sender, EventArgs eventArgs)
        {
            
        }
    }
}
