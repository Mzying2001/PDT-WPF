using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using System.Windows;

namespace PDT_WPF.Views
{
    /// <summary>
    /// TalkTagApplicationManager.xaml 的交互逻辑
    /// </summary>
    public partial class TalkTagApplicationManager : Window
    {
        private static TalkTagApplicationManager openedWinow;

        public TalkTagApplicationManager()
        {
            InitializeComponent();

            Closing += (s, e) => openedWinow = null;

            if (!GlobalData.AdminMode)
            {
                MessageBoxHelper.ShowMessage("当前用户不是管理员账户，无法查看。");
                Close();
            }
        }

        public static new void Show()
        {
            if (openedWinow == null)
            {
                openedWinow = new TalkTagApplicationManager();
                (openedWinow as Window).Show();
            }
            else
            {
                openedWinow.Focus();
            }
        }
    }
}
