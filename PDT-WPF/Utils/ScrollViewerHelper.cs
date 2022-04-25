using System.Windows;
using System.Windows.Input;

namespace PDT_WPF.Utils
{
    public static class ScrollViewerHelper
    {


        public static ICommand GetReachEndCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ReachEndCommandProperty);
        }

        public static void SetReachEndCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ReachEndCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for ReachEndCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReachEndCommandProperty =
            DependencyProperty.RegisterAttached("ReachEndCommand", typeof(ICommand), typeof(ScrollViewerHelper), new PropertyMetadata(null));


    }
}
