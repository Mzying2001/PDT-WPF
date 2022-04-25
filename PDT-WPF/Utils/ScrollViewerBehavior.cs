using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PDT_WPF.Utils
{
    public class ScrollViewerBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ScrollChanged += ScrollChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ScrollChanged -= ScrollChanged;
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset != 0 && e.ExtentHeight - e.VerticalOffset - e.ViewportHeight < 1)
            {
                ScrollViewerHelper.GetReachEndCommand((ScrollViewer)sender)?.Execute(null);
            }
        }
    }
}
