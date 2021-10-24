using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.PageViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public ObservableCollection<string> AdPhotos { get; set; }

        public RelayCommand<string> ShowAdCmd { get; set; }

        private async void GetAdPhotoAsync(Action<PdtV1.GetAdvertisementPhotoResponse> callback)
        {
            try
            {
                var res = await Task.Run(() => PdtV1.GetAdvertisementPhoto());
                callback(res);
            }
            catch
            {
                callback(new PdtV1.GetAdvertisementPhotoResponse
                {
                    isSuccess = false
                });
            }
        }

        private void InitAdPhotos()
        {
            AdPhotos = new ObservableCollection<string>();
            GetAdPhotoAsync(result =>
            {
                if (result.isSuccess)
                {
                    foreach (var item in result.advertisementPhoto)
                    {
                        //if (item.StartsWith("http://") || item.StartsWith("https://"))
                        //    AdPhotos.Add(item);
                        //else
                        //    AdPhotos.Add("https://" + item);


                        for (int i = 0; i < 5; i++) //测试
                        {
                            if (item.StartsWith("http://") || item.StartsWith("https://"))
                                AdPhotos.Add(item);
                            else
                                AdPhotos.Add("https://" + item);
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowError("获取主页轮播图失败！");
                }
            });
        }

        private void ShowAd(string ad)
        {
            MessageBoxHelper.ShowMessage(ad);
        }

        public HomePageViewModel()
        {
            ShowAdCmd = new RelayCommand<string>(ShowAd);

            InitAdPhotos();
        }
    }
}
