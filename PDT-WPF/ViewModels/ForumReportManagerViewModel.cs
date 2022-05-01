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
    public class ForumReportManagerViewModel : ViewModelBase
    {
        public RelayCommand<int> LoadPageCmd { get; set; }
        public RelayCommand LoadNextPageCmd { get; set; }
        public RelayCommand LoadPreviousPageCmd { get; set; }
        public RelayCommand<ForumReport> IgnoreForumPostCmd { get; set; }
        public RelayCommand<ForumReport> DeleteForumPostCmd { get; set; }
        public RelayCommand<string> ViewImageCmd { get; set; }

        public ObservableCollection<ForumReport> ForumReports { get; set; }

        private int _page;
        /// <summary>
        /// 当前页面
        /// </summary>
        public int Page
        {
            get => _page;
            set => Set(ref _page, value);
        }

        /// <summary>
        /// 第一页
        /// </summary>
        public int FirstPage
        {
            get => 1;
        }

        private bool _isLoadingPage;
        /// <summary>
        /// 是否正在加载页面
        /// </summary>
        public bool IsLoadingPage
        {
            get => _isLoadingPage;
            set => Set(ref _isLoadingPage, value);
        }

        private async void LoadPageAsync(int page, Action<PdtV1.GetTipForumResponse> callback)
        {
            try
            {
                IsLoadingPage = true;
                callback(await Task.Run(() => PdtV1.GetTipForum(page)));
            }
            catch (Exception e)
            {
                callback(new PdtV1.GetTipForumResponse { mesg = e.Message });
            }
            finally
            {
                IsLoadingPage = false;
            }
        }

        /// <summary>
        /// 加载需要处理的帖子列表
        /// </summary>
        /// <param name="page"></param>
        private void LoadPage(int page)
        {
            if (ForumReports == null)
                ForumReports = new ObservableCollection<ForumReport>();

            LoadPageAsync(page, result =>
            {
                try
                {
                    if (result.isSuccess)
                    {
                        ForumReports.Clear();
                        foreach (var item in result.tipforums)
                            ForumReports.Add(item);
                        RaisePropertyChanged(nameof(ForumReports));
                        Page = page;
                    }
                    else if (result.mesg == "获取被举报的帖子列表失败:已无更多被举报的帖子")
                    {
                        if (page == FirstPage)
                        {
                            ForumReports.Clear();
                            RaisePropertyChanged(nameof(ForumReports));
                            MessageBoxHelper.ShowMessage("目前没有被举报的帖子。");
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessage("已无更多举报消息。");
                        }
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
        /// 加载下一页
        /// </summary>
        private void LoadNextPage()
        {
            LoadPage(Page + 1);
        }

        /// <summary>
        /// 加载上一页
        /// </summary>
        private void LoadPreviousPage()
        {
            if (Page > FirstPage)
            {
                LoadPage(Page - 1);
            }
            else
            {
                MessageBoxHelper.ShowMessage("已经是第一页了");
            }
        }

        /// <summary>
        /// 忽略举报
        /// </summary>
        /// <param name="forumReport"></param>
        private void IgnoreForumPost(ForumReport forumReport)
        {
            if (MessageBoxHelper.ShowQuestion($"是否忽略对帖子“{forumReport.Forum.Title}”的举报？"))
            {
                try
                {
                    var res = PdtV1.IgnoreForum(forumReport.ID);
                    if (res.isSuccess)
                    {
                        MessageBoxHelper.ShowMessage("已忽略该举报。");
                        if (ForumReports.Count == 1)
                        {
                            if (Page > FirstPage)
                            {
                                LoadPreviousPage();
                            }
                            else
                            {
                                ForumReports.Clear();
                                RaisePropertyChanged(nameof(ForumReports));
                            }
                        }
                        else
                        {
                            LoadPage(Page);
                        }
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
        /// 删除帖子
        /// </summary>
        /// <param name="forumReport"></param>
        private void DeleteForumPost(ForumReport forumReport)
        {
            if (MessageBoxHelper.ShowQuestion($"是否删除帖子“{forumReport.Forum.Title}”？"))
            {
                try
                {
                    var res = PdtV1.DeleteTipForum(forumReport.ID);
                    if (res.isSuccess)
                    {
                        MessageBoxHelper.ShowMessage("已删除该帖子。");
                        if (ForumReports.Count == 1)
                        {
                            if (Page > FirstPage)
                            {
                                LoadPreviousPage();
                            }
                            else
                            {
                                ForumReports.Clear();
                                RaisePropertyChanged(nameof(ForumReports));
                            }
                        }
                        else
                        {
                            LoadPage(Page);
                        }
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
        /// 查看图片
        /// </summary>
        /// <param name="imageSource"></param>
        private void ViewImage(string imageSource)
        {
            Views.HcImageViewer.Show(imageSource);
        }

        public ForumReportManagerViewModel()
        {
            LoadPageCmd = new RelayCommand<int>(LoadPage);
            LoadNextPageCmd = new RelayCommand(LoadNextPage);
            LoadPreviousPageCmd = new RelayCommand(LoadPreviousPage);
            IgnoreForumPostCmd = new RelayCommand<ForumReport>(IgnoreForumPost);
            DeleteForumPostCmd = new RelayCommand<ForumReport>(DeleteForumPost);
            ViewImageCmd = new RelayCommand<string>(ViewImage);

            if (GlobalData.AdminMode)
            {
                Page = FirstPage;
                LoadPage(Page);
            }
        }
    }
}
