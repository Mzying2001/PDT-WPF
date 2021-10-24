using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Models;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.PageViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public ObservableCollection<BoardPhoto> BoardPhotos { get; set; }

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
                    foreach (var item in result)
                    {
                        if (!(item.PhotoUrl.StartsWith("http://") || item.PhotoUrl.StartsWith("https://")))
                        {
                            item.PhotoUrl = "https://" + item.PhotoUrl;
                        }
                        BoardPhotos.Add(item);
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
        }
    }
}
