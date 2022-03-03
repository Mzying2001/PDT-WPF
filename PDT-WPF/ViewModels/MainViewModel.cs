using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Utils;
using PDT_WPF.Views.Pages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace PDT_WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dictionary<string, Page> Pages = new Dictionary<string, Page>
        {
            { "HomePage",           new HomePage()           }, //主页
            { "ProjectLibraryPage", new ProjectLibraryPage() }, //项目库
            { "TalentPoolPage",     new TalentPoolPage()     }, //人才库
            { "ForumPage",          new ForumPage()          }, //论坛
            { "MessagePage",        new MessagePage()        }, //信息
            { "PersonalCenterPage", new PersonalCenterPage() }, //个人中心
            { "AdminPage",          new AdminPage()          }, //后台管理
        };


        public RelayCommand<string> SwitchPageCmd { get; set; }
        public RelayCommand ExitPageCmd { get; set; }
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
        /// 切换页面
        /// </summary>
        /// <param name="pageName"></param>
        private void SwitchPage(string pageName)
        {
            var page = Pages[pageName];
            if (CurrentPage != page)
            {
                CurrentPage = page;

                //通知View层页面被切换
                Messenger.Default.Send(pageName, MessageTokens.PAGE_CHANGED);
            }
        }

        private void ExitPage()
        {
            CurrentPage = null;
            Messenger.Default.Send("", MessageTokens.PAGE_CHANGED);
        }

        private void Logout()
        {
            if (MessageBoxHelper.ShowQuestion($"是否退出账号“{User.NickName}”？"))
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
            SwitchPageCmd = new RelayCommand<string>(SwitchPage);
            ExitPageCmd = new RelayCommand(ExitPage);
            LogoutCmd = new RelayCommand(Logout);

            SwitchPage("HomePage");
        }
    }
}