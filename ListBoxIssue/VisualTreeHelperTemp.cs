using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ListBoxIssue
{
    public static class VisualTreeHelperTemp
    {
        public static T GetVisualChild<T>(this Visual referenceVisual) where T : Visual
        {
            Visual child = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(referenceVisual); i++)
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