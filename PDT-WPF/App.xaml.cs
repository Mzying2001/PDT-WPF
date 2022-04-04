using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using PDT_WPF.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Interop;

namespace PDT_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private const int WM_USER = 0x0400;
        private const int WM_SHOW = WM_USER + 1;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (GetStartedProcess() is Process p)
            {
                //程序已打开
                var pid = WinApi.GetWindowThreadProcessId(p.MainWindowHandle, default);
                WinApi.PostThreadMessage(pid, WM_SHOW, default, default);
                Environment.Exit(0);
            }
            else
            {
                ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessage;

                if (LocalData.Settings.EnableLog)
                {
                    Logger.Open(); //打开Log
                    Logger.WriteLine("打开程序");
                }

                LoginWindow.Show();
            }
        }

        private void ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == WM_SHOW)
            {
                if (GlobalData.IsLogined)
                {
                    Views.MainWindow.Show();
                }
                else
                {
                    LoginWindow.Show();
                }
                handled = true;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            LocalData.SaveAllData();

            if (Logger.Opened)
            {
                Logger.WriteLine("退出程序");
                Logger.Close(); //关闭Log
            }
        }

        private static Process GetStartedProcess()
        {
            Process cur = Process.GetCurrentProcess();
            return (from p in Process.GetProcesses() where p.ProcessName == cur.ProcessName && p.Id != cur.Id select p).FirstOrDefault();
        }
    }
}
