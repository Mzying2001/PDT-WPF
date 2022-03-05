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
    public class ForumExamineToolViewModel : ViewModelBase
    {
        public RelayCommand<int> LoadPageCmd { get; set; }
        public RelayCommand LoadNextPageCmd { get; set; }
        public RelayCommand LoadPreviousPageCmd { get; set; }
        public RelayCommand<ForumPost> AcceptForumPostCmd { get; set; }
        public RelayCommand<ForumPost> RejectForumPostCmd { get; set; }
        public RelayCommand<string> ViewImageCmd { get; set; }

        public ObservableCollection<ForumPost> ForumPosts { get; set; }

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

        private async void LoadPageAsync(int page, Action<PdtV1.GetUnapprovedForumPostsResponse> callback)
        {
            try
            {
                IsLoadingPage = true;
                callback(await Task.Run(() => PdtV1.GetUnapprovedForumPosts(page)));
            }
            catch (Exception e)
            {
                callback(new PdtV1.GetUnapprovedForumPostsResponse { mesg = e.Message });
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
            if (ForumPosts == null)
                ForumPosts = new ObservableCollection<ForumPost>();

            LoadPageAsync(page, result =>
            {
                try
                {
                    if (result.isSuccess)
                    {
                        ForumPosts.Clear();
                        foreach (var item in result.forums)
                            ForumPosts.Add(item);
                        RaisePropertyChanged(nameof(ForumPosts));
                        Page = page;
                    }
                    else if (result.mesg == "获取未操作的论坛列表失败:已无更多帖子")
                    {
                        if (page == FirstPage)
                        {
                            ForumPosts.Clear();
                            RaisePropertyChanged(nameof(ForumPosts));
                            MessageBoxHelper.ShowMessage("目前没有帖子需要审核。");
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessage("已无更多帖子。");
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
        /// 审核通过
        /// </summary>
        /// <param name="forumPost"></param>
        private void AcceptForumPost(ForumPost forumPost)
        {
            if (MessageBoxHelper.ShowQuestion($"是否通过帖子“{forumPost.Title}”？"))
            {
                try
                {
                    var res = PdtV1.ProcessUnapprovedForumPosts(forumPost.ID, true);
                    if (res.isSuccess)
                    {
                        MessageBoxHelper.ShowMessage("已通过该帖子。");
                        if (ForumPosts.Count == 1)
                        {
                            if (Page > FirstPage)
                            {
                                LoadPreviousPage();
                            }
                            else
                            {
                                ForumPosts.Clear();
                                RaisePropertyChanged(nameof(ForumPosts));
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
        /// 审核不通过
        /// </summary>
        /// <param name="forumPost"></param>
        private void RejectForumPost(ForumPost forumPost)
        {
            if (MessageBoxHelper.ShowQuestion($"是否拒绝帖子“{forumPost.Title}”？"))
            {
                try
                {
                    var res = PdtV1.ProcessUnapprovedForumPosts(forumPost.ID, false);
                    if (res.isSuccess)
                    {
                        MessageBoxHelper.ShowMessage("已拒绝该帖子。");
                        if (ForumPosts.Count == 1)
                        {
                            if (Page > FirstPage)
                            {
                                LoadPreviousPage();
                            }
                            else
                            {
                                ForumPosts.Clear();
                                RaisePropertyChanged(nameof(ForumPosts));
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

        public ForumExamineToolViewModel()
        {
            LoadPageCmd = new RelayCommand<int>(LoadPage);
            LoadNextPageCmd = new RelayCommand(LoadNextPage);
            LoadPreviousPageCmd = new RelayCommand(LoadPreviousPage);
            AcceptForumPostCmd = new RelayCommand<ForumPost>(AcceptForumPost);
            RejectForumPostCmd = new RelayCommand<ForumPost>(RejectForumPost);
            ViewImageCmd = new RelayCommand<string>(ViewImage);

            if (GlobalData.AdminMode)
            {
                Page = FirstPage;
                LoadPage(Page);
            }
        }
    }
}
