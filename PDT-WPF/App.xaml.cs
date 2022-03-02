using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using PDT_WPF.Views;
using PDT_WPF.Views.Utils;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace PDT_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (CheckStarted())
            {
                //阻止多开
                MessageBoxHelper.ShowMessage("程序已启动");
                Current.Shutdown();
                return;
            }

            Logger.Open(); //打开Log
            Logger.WriteLine("打开程序");

            bool loadLoginWindow = true;
            foreach (var arg in e.Args)
            {
                switch (arg)
                {
                    //case "-Admin":
                    //    {
                    //        //只启动后台管理界面
                    //        var window = new Window
                    //        {
                    //            Title = "后台管理",
                    //            Background = new SolidColorBrush(Colors.White),
                    //            Content = new AdminPage
                    //            {
                    //                Margin = new Thickness(10, 0, 0, 0)
                    //            }
                    //        };

                    //        loadLoginWindow = false;
                    //        window.Show();
                    //        break;
                    //    }

                    //case "-ShowAdminPage":
                    //    {
                    //        //主窗口显示后台管理页面
                    //        GlobalData.ShowAdminPage = true;
                    //        break;
                    //    }

                    default:
                        {
                            MessageBoxHelper.ShowError($"无效的启动参数：{arg}。");
                            break;
                        }
                }
            }

            if (loadLoginWindow)
                new LoginWindow().Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            LocalData.SaveAllData();

            Logger.WriteLine("退出程序");
            Logger.Close(); //关闭Log
        }

        private static bool CheckStarted()
        {
            Process cur = Process.GetCurrentProcess();
            return (from p in Process.GetProcesses() where p.ProcessName == cur.ProcessName && p.Id != cur.Id select p).Any();
        }
    }
}
