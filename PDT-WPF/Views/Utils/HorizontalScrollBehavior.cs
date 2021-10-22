﻿using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PDT_WPF.Views.Utils
{
    /// <summary>
    /// 使组件所在的ScrollViewer实现使用鼠标滚轮横向滚动页面
    /// </summary>
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
                int d = e.Delta;
                if (d > 0)
                {
                    scroll.LineLeft();
                }
                else if (d < 0)
                {
                    scroll.LineRight();
                }
                scroll.ScrollToTop();
            }
        }
    }
}
