using PDT_WPF.Log;
using PDT_WPF.Models.Data;
using PDT_WPF.Views;
using PDT_WPF.Views.Utils;
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
    }
}
