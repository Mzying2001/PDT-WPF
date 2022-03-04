using System.Windows;

namespace PDT_WPF.Views
{
    /// <summary>
    /// HcImageViewer.xaml 的交互逻辑
    /// </summary>
    public partial class HcImageViewer : Window
    {


        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(HcImageViewer), new PropertyMetadata(null));


        public HcImageViewer()
        {
            InitializeComponent();
        }

        public static void Show(string imageSource)
        {
            new HcImageViewer { ImageSource = imageSource }.Show();
        }
    }
}
