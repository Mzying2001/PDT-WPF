using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PDT_WPF.Views.Utils
{
    /// <summary>
    /// 使组件所在的ScrollViewer实现使用鼠标滚轮横向滚动页面
    /// </summary>
    [Obsolete("使用HandyControl的ScrollViewer替换")]
    public class HorizontalScrollBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseWheel += MouseWheel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseWheel -= MouseWheel;
        }


        /// <summary>
        /// 实现鼠标滚动时内容横向滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ItemsControl items = sender as ItemsControl;
            ScrollViewer scroll = MyVisualTreeHelper.GetAncestor<ScrollViewer>(items);

            if (items != null && scroll != null)
            {
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
                scroll.ScrollToTop();
            }
        }
    }
}
