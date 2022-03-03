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
            { "HomePage",           new HomePage()           }, //��ҳ
            { "ProjectLibraryPage", new ProjectLibraryPage() }, //��Ŀ��
            { "TalentPoolPage",     new TalentPoolPage()     }, //�˲ſ�
            { "ForumPage",          new ForumPage()          }, //��̳
            { "MessagePage",        new MessagePage()        }, //��Ϣ
            { "PersonalCenterPage", new PersonalCenterPage() }, //��������
            { "AdminPage",          new AdminPage()          }, //��̨����
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
        /// �л�ҳ��
        /// </summary>
        /// <param name="pageName"></param>
        private void SwitchPage(string pageName)
        {
            var page = Pages[pageName];
            if (CurrentPage != page)
            {
                CurrentPage = page;

                //֪ͨView��ҳ�汻�л�
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
            if (MessageBoxHelper.ShowQuestion($"�Ƿ��˳��˺š�{User.NickName}����"))
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