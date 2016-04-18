using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
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

        public static readonly DependencyProperty SetScrollbarPositionProperty = DependencyProperty.Register("SetScrollbarPosition", typeof (int), typeof (ScrollListBoxBehavior), new FrameworkPropertyMetadata(ScrollBarPositionCallback));

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
}
