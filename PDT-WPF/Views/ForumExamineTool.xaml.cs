using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using System.Windows;

namespace PDT_WPF.Views
{
    /// <summary>
    /// ForumExamineTool.xaml 的交互逻辑
    /// </summary>
    public partial class ForumExamineTool : Window
    {
        private static ForumExamineTool openedWindow;

        public ForumExamineTool()
        {
            InitializeComponent();
            Closing += (s, e) => openedWindow = null;

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
                openedWindow = new ForumExamineTool();
                (openedWindow as Window).Show();
            }
            else
            {
                openedWindow.Focus();
            }
        }
    }
}
