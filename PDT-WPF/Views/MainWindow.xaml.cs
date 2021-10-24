using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PDT_WPF.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //切换页面时更新SideMenu选中项
            Messenger.Default.Register<string>(this, MessageTokens.PAGE_CHANGED, UpdateSideMenuSelected);
            Unloaded += (s, e) => Messenger.Default.Unregister<string>(this);

            //退出登录时View层响应
            Messenger.Default.Register<object>(this, MessageTokens.LOGOUT, Logout);
            Unloaded += (s, e) => Messenger.Default.Unregister<object>(this);
        }

        /// <summary>
        /// ViewModel中Page更改时更新SideMenu选中项
        /// </summary>
        /// <param name="pageName"></param>
        private void UpdateSideMenuSelected(string pageName)
        {
            foreach (var item in sideMenu.Items)
            {
                if (item is HandyControl.Controls.SideMenuItem sideMenuItem)
                    sideMenuItem.IsSelected = sideMenuItem.CommandParameter.Equals(pageName);
            }
        }

        /// <summary>
        /// 退出登录时View层响应
        /// </summary>
        /// <param name="obj"></param>
        private void Logout(object obj)
        {
            new LoginWindow().Show();
            Close();
        }

        /// <summary>
        /// 左上角用户头像单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            if (Resources["UserContextMenu"] is ContextMenu menu)
            {
                menu.PlacementTarget = sender as UIElement;
                menu.IsOpen = true;
            }
        }
    }
}
