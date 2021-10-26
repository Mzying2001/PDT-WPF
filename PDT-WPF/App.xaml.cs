using PDT_WPF.Models.Data;
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

            foreach (var arg in e.Args)
            {
                switch (arg)
                {
                    case "-ShowAdminPage":
                        {
                            GlobalData.ShowAdminPage = true;
                        }
                        break;

                    default:
                        {
                            MessageBoxHelper.ShowError($"无效的启动参数：{arg}。");
                        }
                        break;
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            LocalData.SaveAllData();
        }
    }
}
