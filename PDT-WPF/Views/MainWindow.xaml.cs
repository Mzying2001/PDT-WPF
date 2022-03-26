using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PDT_WPF.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Window openedWindow;

        public MainWindow()
        {
            InitializeComponent();

            //恢复上次关闭时的位置尺寸
            if (LocalData.Settings.MainWindowSizeInfo != null)
                LocalData.Settings.MainWindowSizeInfo.Apply(this);

            //默认打开主界面
            if (DataContext is ViewModels.MainViewModel vm)
            {
                vm.CurrentPage = Resources["HomePage"] as Page;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            //切换页面后更新侧边栏选中
            foreach (var item in sideMenu.Items)
            {
                if (item is HandyControl.Controls.SideMenuItem sideMenuItem)
                    sideMenuItem.IsSelected = ReferenceEquals(sideMenuItem.CommandParameter, mainFrame.Content);
            }
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

        protected override void OnClosing(CancelEventArgs e)
        {
            //保存窗口尺寸和位置信息
            if (WindowState != WindowState.Normal)
                WindowState = WindowState.Normal;
            LocalData.Settings.MainWindowSizeInfo = WindowSizeInfo.GetSizeInfo(this);

            openedWindow = null;
            base.OnClosing(e);
        }

        public static new void Show()
        {
            if (openedWindow == null)
            {
                openedWindow = new MainWindow();
                openedWindow.Show();
            }
            else
            {
                WindowHelper.SetForeground(openedWindow);
            }
        }
    }
}
