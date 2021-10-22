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
    }
}
