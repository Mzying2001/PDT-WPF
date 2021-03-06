using PDT_WPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDT_WPF.Views.Pages
{
    /// <summary>
    /// AdminPage.xaml 的交互逻辑
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private Expander[] GetExpanders(DependencyObject parent)
        {
            return MyVisualTreeHelper.GetChildren<Expander>(parent);
        }

        private void ExpanderExpanded(object sender, RoutedEventArgs e)
        {
            foreach (var item in GetExpanders(sender as DependencyObject))
            {
                if (!ReferenceEquals(e.Source, item))
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            foreach (var item in GetExpanders(sender as DependencyObject))
            {
                item.Visibility = Visibility.Visible;
            }
        }
    }
}
