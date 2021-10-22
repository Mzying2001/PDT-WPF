using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Views.Utils;
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
using System.Windows.Shapes;

namespace PDT_WPF.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<LoginResult>(this, MessageTokens.LOGIN_RESULT, ProcessLoginResult);
            Unloaded += (s, e) => Messenger.Default.Unregister(this);
        }

        private void ProcessLoginResult(LoginResult result)
        {
            if (result.Success)
            {
                new MainWindow().Show();
                Close();
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message, "登录失败");
            }
        }
    }
}
