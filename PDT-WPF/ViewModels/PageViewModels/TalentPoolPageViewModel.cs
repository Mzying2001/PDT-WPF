using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Models;
using PDT_WPF.Services.Api;
using PDT_WPF.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.PageViewModels
{
    public class TalentPoolPageViewModel : ViewModelBase
    {
        public ObservableCollection<Personnel> Personnels { get; set; }

        public RelayCommand LoadMorePersonnelsCmd { get; set; }
        public RelayCommand ReloadPersonnelListCmd { get; set; }
        public RelayCommand<Personnel> ShowPersonnelCmd { get; set; }


        private int Page { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                Set(ref _isLoading, value);
                LoadMorePersonnelsCmd.RaiseCanExecuteChanged();
                ReloadPersonnelListCmd.RaiseCanExecuteChanged();
            }
        }

        private bool _allLoaded;
        public bool AllLoaded
        {
            get => _allLoaded;
            set => Set(ref _allLoaded, value);
        }


        private async void GetPersonnelListAsync(Action<PdtV1.GetPersonnelListResponse> callback)
        {
            IsLoading = true;
            try
            {
                var res = await Task.Run(() => PdtV1.GetPersonnelList((++Page).ToString(), "0"));
                if (!res.isSuccess)
                    Page--;
                callback(res);
            }
            catch (Exception e)
            {
                Page--;
                callback(new PdtV1.GetPersonnelListResponse
                {
                    isSuccess = false,
                    mesg = e.Message
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void InitPersonnels()
        {
            Personnels = new ObservableCollection<Personnel>();
            LoadMorePersonnels();
        }

        private void LoadMorePersonnels()
        {
            if (IsLoading)
                return;

            GetPersonnelListAsync(result =>
            {
                if (result.isSuccess)
                {
                    foreach (var item in result.personnelList)
                        Personnels.Add(item);
                }
                else
                {
                    if (result.mesg == "获取人才信息列表失败:已无更多人才")
                    {
                        //MessageBoxHelper.ShowMessage("已经全部加载。");
                        AllLoaded = true;
                    }
                    else
                    {
                        MessageBoxHelper.ShowError(result.mesg);
                    }
                }
            });
        }

        private void ReloadPersonnelList()
        {
            if (IsLoading)
                return;

            AllLoaded = false;

            int tmp = Page;
            Page = 0;
            GetPersonnelListAsync(result =>
            {
                if (result.isSuccess)
                {
                    Personnels.Clear();
                    foreach (var item in result.personnelList)
                        Personnels.Add(item);
                }
                else
                {
                    Page = tmp;
                    MessageBoxHelper.ShowError(result.mesg);
                }
            });
        }

        private void ShowPersonnel(Personnel personnel)
        {
            MessageBoxHelper.ShowMessage(Newtonsoft.Json.JsonConvert.SerializeObject(personnel));
        }

        public TalentPoolPageViewModel()
        {
            LoadMorePersonnelsCmd = new RelayCommand(LoadMorePersonnels, () => !IsLoading);
            ReloadPersonnelListCmd = new RelayCommand(ReloadPersonnelList, () => !IsLoading);
            ShowPersonnelCmd = new RelayCommand<Personnel>(ShowPersonnel);

            InitPersonnels();
        }
    }
}
