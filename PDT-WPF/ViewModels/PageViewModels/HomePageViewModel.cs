using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Dialogs;
using PDT_WPF.Views.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.PageViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public ObservableCollection<BoardPhoto> BoardPhotos { get; set; }
        public ObservableCollection<List<CompetitionSection>> CompetitionSections { get; set; }

        public RelayCommand<BoardPhoto> OpenBoardPhotoLinkCmd { get; set; }
        public RelayCommand<CompetitionSection> ShowCompetitionSectionCmd { get; set; }

        private async void GetBoardPhotosAsync(Action<BoardPhoto[]> callback)
        {
            try
            {
                callback(await Task.Run(() => PdtV2.GetBoardPhotos()));
            }
            catch
            {
                callback(null);
            }
        }

        private void LoadBoardPhotos()
        {
            if (BoardPhotos == null)
                BoardPhotos = new ObservableCollection<BoardPhoto>();

            GetBoardPhotosAsync(result =>
            {
                if (result != null)
                {
                    BoardPhotos.Clear();
                    foreach (var item in result)
                        BoardPhotos.Add(item);
                }
            });
        }

        private async void GetCompetitionSectionsAsync(Action<CompetitionSection[][]> callback)
        {
            try
            {
                callback(await Task.Run(() => PdtV2.GetCompetitionSections()));
            }
            catch
            {
                callback(null);
            }
        }

        private void LoadCompetitionSections()
        {
            if (CompetitionSections == null)
                CompetitionSections = new ObservableCollection<List<CompetitionSection>>();

            GetCompetitionSectionsAsync(result =>
            {
                if (result != null)
                {
                    CompetitionSections.Clear();

                    foreach (var arr in result)
                    {
                        if (arr.Length == 0)
                            continue;

                        var list = new List<CompetitionSection>(arr.Length);
                        foreach (var item in arr)
                            list.Add(item);
                        CompetitionSections.Add(list);
                    }
                }
            });
        }

        private void OpenBoardPhotoLink(BoardPhoto boardPhoto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(boardPhoto.Link))
                    return;

                switch (boardPhoto.Jump)
                {
                    case BoardPhoto.JumpType.NoJump:
                        break;

                    case BoardPhoto.JumpType.JumpLink:
                        {
                            Process.Start(boardPhoto.Link);
                        }
                        break;

                    case BoardPhoto.JumpType.JumpMiniProgram:
                        {
                            MessageBoxHelper.ShowMessage($"该链接用于打开另外一个小程序，需要在微信小程序里打开。\n\n目标链接：\n{boardPhoto.Link}");
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBoxHelper.ShowError(e);
            }
        }

        private void ShowCompetitionSection(CompetitionSection competitionSection)
        {
            CompetitionSectionDetailDialog.ShowDialog(competitionSection);
        }

        /// <summary>
        /// 更新主页显示的数据
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateData(object obj = null)
        {
            LoadBoardPhotos();
            LoadCompetitionSections();
        }

        public HomePageViewModel()
        {
            OpenBoardPhotoLinkCmd = new RelayCommand<BoardPhoto>(OpenBoardPhotoLink);
            ShowCompetitionSectionCmd = new RelayCommand<CompetitionSection>(ShowCompetitionSection);

            UpdateData();
            Messenger.Default.Register<object>(this, MessageTokens.HOMEPAGE_DATA_UPDATED, UpdateData);
        }

        ~HomePageViewModel()
        {
            Messenger.Default.Unregister(this);
        }
    }
}
