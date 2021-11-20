using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PDT_WPF.Views.Panels
{
    public class WaterFallPanel : Panel
    {


        public double ItemMinWidth
        {
            get { return (double)GetValue(ItemMinWidthProperty); }
            set { SetValue(ItemMinWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemMinWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemMinWidthProperty =
            DependencyProperty.Register("ItemMinWidth", typeof(double), typeof(WaterFallPanel), new PropertyMetadata(50d));



        private static int AppendMin(double[] arr, double value)
        {
            if (arr.Length > 0)
            {
                int minIndex = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] < arr[minIndex])
                        minIndex = i;
                }
                arr[minIndex] += value;
                return minIndex;
            }
            return -1;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size();

            if (double.IsPositiveInfinity(availableSize.Width))
            {
                var infinity = new Size(double.PositiveInfinity, double.PositiveInfinity);
                var widthSum = 0d;
                var maxHeight = 0d;

                foreach (UIElement child in Children)
                {
                    child.Measure(infinity);
                    widthSum += child.DesiredSize.Width;
                    maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
                }

                size = new Size(widthSum, maxHeight);
            }
            else if (Children.Count > 0)
            {
                var rowCount = (int)Math.Min(Children.Count, Math.Floor(availableSize.Width / ItemMinWidth));
                if (rowCount == 0)
                    rowCount = 1;

                var itemWith = availableSize.Width > ItemMinWidth ? availableSize.Width / rowCount : availableSize.Width;
                var bottoms = new double[rowCount];

                foreach (UIElement child in Children)
                {
                    child.Measure(new Size(itemWith, double.PositiveInfinity));
                    AppendMin(bottoms, child.DesiredSize.Height);
                }

                size = new Size(availableSize.Width, bottoms.Max());
            }

            if (!double.IsPositiveInfinity(availableSize.Height))
                size.Height = Math.Max(availableSize.Height, size.Height);

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = new Size(finalSize.Width, 0);
            var rowCount = (int)Math.Min(Children.Count, Math.Floor(finalSize.Width / ItemMinWidth));
            if (rowCount == 0)
                rowCount = 1;

            var itemWith = finalSize.Width > ItemMinWidth ? finalSize.Width / rowCount : finalSize.Width;
            var bottoms = new double[rowCount];

            foreach (UIElement child in Children)
            {
                int index = AppendMin(bottoms, child.DesiredSize.Height);
                child.Arrange(new Rect(index * itemWith, bottoms[index] - child.DesiredSize.Height, itemWith, child.DesiredSize.Height));
            }

            size.Height = bottoms.Max();
            return size;
        }
    }
}
