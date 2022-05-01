using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using System.Windows;

namespace PDT_WPF.Views
{
    /// <summary>
    /// ForumReportManager.xaml 的交互逻辑
    /// </summary>
    public partial class ForumReportManager : Window
    {
        private static Window openedWindow;

        public ForumReportManager()
        {
            InitializeComponent();
            Closing += delegate { openedWindow = null; };

            if (!GlobalData.AdminMode)
            {
                MessageBoxHelper.ShowMessage("当前用户没有足够的权限。");
                Close();
            }
        }

        public static new void Show()
        {
            if (openedWindow == null)
            {
                openedWindow = new ForumReportManager();
                openedWindow.Show();
            }
            else
            {
                openedWindow.Focus();
            }
        }
    }
}
