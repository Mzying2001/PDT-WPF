using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using System.Windows;

namespace PDT_WPF.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private static Window openedWindow;

        public LoginWindow()
        {
            InitializeComponent();

            accountBox.Focus();
            accountBox.SelectAll();

            Closing += (s, e) => openedWindow = null;

            Messenger.Default.Register<LoginResult>(this, MessageTokens.LOGIN_RESULT, ProcessLoginResult);
            Unloaded += (s, e) => Messenger.Default.Unregister(this);
        }

        private void ProcessLoginResult(LoginResult result)
        {
            if (result.Success)
            {
                MainWindow.Show();
                Close();
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message, "登录失败");
            }
        }

        public static new void Show()
        {
            if (openedWindow == null)
            {
                openedWindow = new LoginWindow();
                openedWindow.Show();
            }
            else
            {
                WindowHelper.SetForeground(openedWindow);
            }
        }
    }
}
