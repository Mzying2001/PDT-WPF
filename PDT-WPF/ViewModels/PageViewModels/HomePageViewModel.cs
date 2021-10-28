using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Models;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.PageViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public ObservableCollection<BoardPhoto> BoardPhotos { get; set; }
        public ObservableCollection<List<CompetitionSection>> CompetitionSections { get; set; }

        public RelayCommand<string> OpenLinkCmd { get; set; }

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
                        var list = new List<CompetitionSection>(arr.Length);
                        foreach (var item in arr)
                            list.Add(item);
                        CompetitionSections.Add(list);
                    }
                }
            });
        }

        private void OpenLink(string link)
        {
            try
            {
                System.Diagnostics.Process.Start(link);
            }
            catch (Exception e)
            {
                MessageBoxHelper.ShowError(e);
            }
        }

        public HomePageViewModel()
        {
            OpenLinkCmd = new RelayCommand<string>(OpenLink);

            LoadBoardPhotos();
            LoadCompetitionSections();
        }
    }
}
