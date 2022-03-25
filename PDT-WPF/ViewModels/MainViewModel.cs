using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace PDT_WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<Page> SwitchPageCmd { get; set; }
        public RelayCommand LogoutCmd { get; set; }


        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set => Set(ref _currentPage, value);
        }

        public User User
        {
            get => GlobalData.CurrentUser;
            set
            {
                GlobalData.CurrentUser = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// ÇÐ»»Ò³Ãæ
        /// </summary>
        /// <param name="page"></param>
        private void SwitchPage(Page page)
        {
            CurrentPage = page;
        }

        private void Logout()
        {
            if (MessageBoxHelper.ShowQuestion($"ÊÇ·ñÍË³öÕËºÅ¡°{User.NickName}¡±£¿"))
            {
                LocalData.Settings.OpenId = string.Empty;
                LocalData.SaveAllData();
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();

                //LocalData.Settings.OpenId = string.Empty;
                //Messenger.Default.Send<object>(null, MessageTokens.LOGOUT);
            }
        }


        public MainViewModel()
        {
            SwitchPageCmd = new RelayCommand<Page>(SwitchPage);
            LogoutCmd = new RelayCommand(Logout);
        }
    }
}