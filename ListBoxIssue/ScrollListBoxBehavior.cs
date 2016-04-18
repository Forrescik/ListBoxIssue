using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using ListBoxIssue.ViewModel;

namespace ListBoxIssue
{
    public class ScrollListBoxBehavior : Behavior<ListBox>
    {
        private ScrollViewer _scrollViewer;
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            base.OnAttached();
        }

        public static readonly DependencyProperty IsOnTopProperty = DependencyProperty.Register("IsOnTop", typeof(RelayCommand), typeof(ScrollListBoxBehavior));

        public RelayCommand IsOnTop
        {
            get { return (RelayCommand) GetValue(IsOnTopProperty); }
            set { SetValue(IsOnTopProperty, value);}
        }

        public static readonly DependencyProperty SetScrollbarPositionProperty = DependencyProperty.Register("SetScrollbarPosition", typeof(int), typeof(ScrollListBoxBehavior), new FrameworkPropertyMetadata(ScrollBarPositionCallback));

        public int SetScrollbarPosition
        {
            get { return (int) GetValue(SetScrollbarPositionProperty); }
            set { SetValue(SetScrollbarPositionProperty, value);}
        }

        public static readonly DependencyProperty IsScrollFluentProperty = DependencyProperty.Register("IsScrollFluent", typeof(bool), typeof(ScrollListBoxBehavior), new PropertyMetadata(false));

        public bool IsScrollFluent
        {
            get { return (bool) GetValue(IsScrollFluentProperty); }
            set { SetValue(IsScrollFluentProperty, value);}
        }

        private static void ScrollBarPositionCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _scrollViewer = AssociatedObject.GetVisualChild<ScrollViewer>();
            _scrollViewer.ScrollToBottom();
            _scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
            if (_scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Collapsed)
            {
                ScrollViewerOnScrollChanged(this, null);
            }
        }

        private int _lastKnownPosition;

        private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs scrollChangedEventArgs)
        {
            if ((int) _scrollViewer.ContentVerticalOffset == 0 && _lastKnownPosition != 0)
            {
                if (IsOnTop != null)
                {
                    IsOnTop.Execute(null);
                    IsScrollFluent = true;
                    var item = AssociatedObject.Items.GetItemAt(SetScrollbarPosition);
                    AssociatedObject.ScrollIntoViewCentered(item);
                    AssociatedObject.Items.MoveCurrentTo(item);
                    IsScrollFluent = false;

                }
            }
            _lastKnownPosition = (int)_scrollViewer.ContentVerticalOffset;
            for (int i = 0; i < AssociatedObject.Items.Count; i++)
            {
                ListBoxItem item = AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                var content = item?.Content as MessageModel;
                if (content != null && !content.IsRead)
                {
                    if (ViewportHelper.IsInViewport(item))
                    {
                        content.SetIsRead();
                        Trace.WriteLine(content.Description);
                    }
                }

            }
        }


        protected override void OnDetaching()
        {
            _scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
            base.OnDetaching();
        }


        public static class ViewportHelper
        {
            public static bool IsInViewport(Control item)
            {
                if (item == null) return false;
                ItemsControl itemsControl;
                if (item is ListBoxItem)
                {
                    itemsControl = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
                }
                else
                {
                    throw new NotSupportedException(item.GetType().Name);
                }

                ScrollViewer scrollViewer = VisualTreeHelperTemp.GetVisualChild<ScrollViewer, ItemsControl>(itemsControl);
                ScrollContentPresenter scrollContentPresenter = (ScrollContentPresenter)scrollViewer.Template.FindName("PART_ScrollContentPresenter", scrollViewer);
                MethodInfo isInViewportMethod = scrollViewer.GetType().GetMethod("IsInViewport", BindingFlags.NonPublic | BindingFlags.Instance);

                return (bool)isInViewportMethod.Invoke(scrollViewer, new object[] { scrollContentPresenter, item });
            }
        }


    }

    public static class ListBoxExtension
    {
        public static void ScrollIntoViewCentered(this ListBox listBox, object item)
        {
            Debug.Assert(!VirtualizingPanel.GetIsVirtualizing(listBox),
                "VirtualizingStackPanel.IsVirtualizing must be disabled for ScrollIntoViewCentered to work.");

            // Get the container for the specified item
            var container = listBox.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
            if (null != container)
            {
                if (ScrollViewer.GetCanContentScroll(listBox))
                {
                    // Get the parent IScrollInfo
                    var scrollInfo = VisualTreeHelper.GetParent(container) as IScrollInfo;
                    if (null != scrollInfo)
                    {
                        // Need to know orientation, so parent must be a known type
                        var stackPanel = scrollInfo as StackPanel;
                        var virtualizingStackPanel = scrollInfo as VirtualizingStackPanel;
                        Debug.Assert((null != stackPanel) || (null != virtualizingStackPanel),
                            "ItemsPanel must be a StackPanel or VirtualizingStackPanel for ScrollIntoViewCentered to work.");

                        // Get the container's index
                        var index = listBox.ItemContainerGenerator.IndexFromContainer(container);

                        // Center the item by splitting the extra space
                        if (((null != stackPanel) && (Orientation.Horizontal == stackPanel.Orientation)) ||
                            ((null != virtualizingStackPanel) && (Orientation.Horizontal == virtualizingStackPanel.Orientation)))
                        {
                            scrollInfo.SetHorizontalOffset(index - Math.Floor(scrollInfo.ViewportWidth / 2));
                        }
                        else
                        {
                            scrollInfo.SetVerticalOffset(index - Math.Floor(scrollInfo.ViewportHeight / 2));
                        }
                    }
                }
                else
                {
                    // Get the bounds of the item container
                    var rect = new Rect(new Point(), container.RenderSize);

                    // Find constraining parent (either the nearest ScrollContentPresenter or the ListBox itself)
                    FrameworkElement constrainingParent = container;
                    do
                    {
                        constrainingParent = VisualTreeHelper.GetParent(constrainingParent) as FrameworkElement;
                    } while ((null != constrainingParent) &&
                             (!Equals(listBox, constrainingParent)) &&
                             !(constrainingParent is ScrollContentPresenter));

                    if (null != constrainingParent)
                    {
                        // Inflate rect to fill the constraining parent
                        rect.Inflate(
                            Math.Max((constrainingParent.ActualWidth - rect.Width) / 2, 0),
                            Math.Max((constrainingParent.ActualHeight - rect.Height) / 2, 0));
                    }

                    // Bring the (inflated) bounds into view
                    container.BringIntoView(rect);
                }
            }
        }

    }

    public static class VisualTreeHelperTemp
    {
        public static T GetVisualChild<T>(this Visual referenceVisual) where T : Visual
        {
            Visual child = null;
            for (Int32 i = 0; i < VisualTreeHelper.GetChildrenCount(referenceVisual); i++)
            {
                child = VisualTreeHelper.GetChild(referenceVisual, i) as Visual;
                if (child is T)
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetVisualChild<T>(child);
                    if (child != null)
                    {
                        break;
                    }
                }
            }
            return child as T;
        }

        private static void GetVisualChildren<T>(DependencyObject current, Collection<T> children) where T : DependencyObject
        {
            if (current != null)
            {
                if (current.GetType() == typeof(T))
                {
                    children.Add((T)current);
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    GetVisualChildren(VisualTreeHelper.GetChild(current, i), children);
                }
            }
        }

        public static Collection<T> GetVisualChildren<T>(DependencyObject current) where T : DependencyObject
        {
            if (current == null)
            {
                return null;
            }

            Collection<T> children = new Collection<T>();

            GetVisualChildren(current, children);

            return children;
        }

        public static T GetVisualChild<T, TP>(TP templatedParent)
             where T : FrameworkElement
             where TP : FrameworkElement
        {
            Collection<T> children = GetVisualChildren<T>(templatedParent);

            return children.FirstOrDefault(child => Equals(child.TemplatedParent, templatedParent));
        }

    }


}
