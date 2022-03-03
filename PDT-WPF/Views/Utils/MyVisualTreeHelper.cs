using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PDT_WPF.Views.Utils
{
    public static class MyVisualTreeHelper
    {
        public static T GetAncestor<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null && !(obj is T))
                obj = VisualTreeHelper.GetParent(obj);
            return obj as T;
        }

        public static T[] GetChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            List<T> result = new List<T>();
            for (int i = VisualTreeHelper.GetChildrenCount(obj) - 1; i >= 0; i--)
            {
                var item = VisualTreeHelper.GetChild(obj, i);
                if (item is T child)
                    result.Add(child);
            }
            return result.ToArray();
        }
    }
}
