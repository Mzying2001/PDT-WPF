using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels
{
    public class TalkTagApplicationManagerViewModel : ViewModelBase
    {
        public RelayCommand LoadTalkTagApplicationsCmd { get; set; }
        public RelayCommand<TalkTagItem> AcceptTalkTagCmd { get; set; }
        public RelayCommand<TalkTagItem> RejectTalkTagCmd { get; set; }

        public ObservableCollection<TalkTagItem> TalkTagApplications { get; set; }

        private bool _isLoading;
        /// <summary>
        /// 是否正在加载话题标签申请列表
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                Set(ref _isLoading, value);
                LoadTalkTagApplicationsCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 加载话题标签申请列表的异步方法
        /// </summary>
        public async void LoadTalkTagApplicationsAsync(Action<PdtV1.GetForumTalkTagApplyListResponse> callback)
        {
            try
            {
                IsLoading = true;
                callback(await Task.Run(() => PdtV1.GetForumTalkTagApplyList()));
            }
            catch
            {
                callback(default);
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 加载话题标签申请列表
        /// </summary>
        private void LoadTalkTagApplications()
        {
            if (TalkTagApplications == null)
                TalkTagApplications = new ObservableCollection<TalkTagItem>();

            if (IsLoading)
                return;

            LoadTalkTagApplicationsAsync(result =>
            {
                try
                {
                    if (result.isSuccess)
                    {
                        TalkTagApplications.Clear();
                        foreach (var item in result.talkTags)
                            TalkTagApplications.Add(item);
                        RaisePropertyChanged(nameof(TalkTagApplications));
                    }
                    else
                    {
                        throw new Exception(result.mesg);
                    }
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e);
                }
            });
        }

        /// <summary>
        /// 接受话题标签
        /// </summary>
        /// <param name="talkTag"></param>
        private void AcceptTalkTag(TalkTagItem talkTag)
        {
            if (MessageBoxHelper.ShowQuestion($"是否接受话题标签“{talkTag.TalkTag}”？"))
            {
                try
                {
                    var res = PdtV1.ProcessTalkTagAppy(talkTag.TalkTagId, true);
                    if (res.isSuccess)
                    {
                        LoadTalkTagApplications();
                        MessageBoxHelper.ShowMessage($"已接受“{talkTag.TalkTag}”。");
                    }
                    else
                    {
                        throw new Exception(res.mesg);
                    }
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e);
                }
            }
        }

        /// <summary>
        /// 拒绝话题标签
        /// </summary>
        /// <param name="talkTag"></param>
        private void RejectTalkTag(TalkTagItem talkTag)
        {
            if (MessageBoxHelper.ShowQuestion($"是否拒绝话题标签“{talkTag.TalkTag}”？"))
            {
                try
                {
                    var res = PdtV1.ProcessTalkTagAppy(talkTag.TalkTagId, false);
                    if (res.isSuccess)
                    {
                        LoadTalkTagApplications();
                        MessageBoxHelper.ShowMessage($"已拒绝“{talkTag.TalkTag}”。");
                    }
                    else
                    {
                        throw new Exception(res.mesg);
                    }
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e);
                }
            }
        }

        public TalkTagApplicationManagerViewModel()
        {
            LoadTalkTagApplicationsCmd = new RelayCommand(LoadTalkTagApplications, () => !IsLoading);
            AcceptTalkTagCmd = new RelayCommand<TalkTagItem>(AcceptTalkTag);
            RejectTalkTagCmd = new RelayCommand<TalkTagItem>(RejectTalkTag);

            if (GlobalData.AdminMode)
            {
                LoadTalkTagApplications();
            }
        }
    }
}
