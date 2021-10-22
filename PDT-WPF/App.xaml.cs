using PDT_WPF.Models;
using System.Windows;

namespace PDT_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            LocalData.SaveAllData();
        }
    }
}
