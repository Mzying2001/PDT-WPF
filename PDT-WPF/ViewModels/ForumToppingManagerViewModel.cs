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
    public class ForumToppingManagerViewModel : ViewModelBase
    {
        private int currentPage = 1;

        public RelayCommand RefreshCmd { get; set; }
        public RelayCommand LoadMorePostsCmd { get; set; }
        public RelayCommand<ForumPost> AddTopForumPostCmd { get; set; }
        public RelayCommand<ForumPost> CancelTopForumPostCmd { get; set; }

        public ObservableCollection<ForumPost> ForumPosts { get; set; }
        public ObservableCollection<ForumPost> TopForumPosts { get; set; }

        public bool IsLoading
        {
            get => IsLoadingForum || IsLoadingTop;
        }

        private bool _isLoadingForum;
        public bool IsLoadingForum
        {
            get => _isLoadingForum;
            set
            {
                Set(ref _isLoadingForum, value);
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        private bool _isLoadingTop;
        public bool IsLoadingTop
        {
            get => _isLoadingTop;
            set
            {
                Set(ref _isLoadingTop, value);
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        private async void LoadForumPostsAsync(int page, Action<PdtV1.GetForumResponse> callback)
        {
            try
            {
                IsLoadingForum = true;
                callback(await Task.Run(() => PdtV1.GetForum(page, 0)));
            }
            catch (Exception e)
            {
                callback(new PdtV1.GetForumResponse { mesg = e.Message });
            }
            finally
            {
                IsLoadingForum = false;
            }
        }

        private async void LoadTopPostsAsync(Action<PdtV1.GetTopForumResponse> callback)
        {
            try
            {
                IsLoadingTop = true;
                callback(await Task.Run(() => PdtV1.GetTopForum()));
            }
            catch (Exception e)
            {
                callback(new PdtV1.GetTopForumResponse { mesg = e.Message });
            }
            finally
            {
                IsLoadingTop = false;
            }
        }

        private void Refresh()
        {
            LoadForumPostsAsync(1, result =>
            {
                if (result.isSuccess)
                {
                    currentPage = 1;
                    ForumPosts.Clear();
                    foreach (var item in result.forums)
                        ForumPosts.Add(item);
                }
                else
                {
                    MessageBoxHelper.ShowError(result.mesg);
                }
            });
            LoadTopPostsAsync(result =>
            {
                if (result.isSuccess)
                {
                    TopForumPosts.Clear();
                    foreach (var item in result.topforums)
                        TopForumPosts.Add(item);
                }
                else
                {
                    MessageBoxHelper.ShowError(result.mesg);
                }
            });
        }

        private void LoadMorePosts()
        {
            LoadForumPostsAsync(currentPage + 1, result =>
            {
                if (result.isSuccess)
                {
                    currentPage++;
                    foreach (var item in result.forums)
                        ForumPosts.Add(item);
                }
                else if (result.mesg == "获取论坛列表失败:已无更多帖子")
                {
                    MessageBoxHelper.ShowMessage("已无更多帖子");
                }
                else
                {
                    MessageBoxHelper.ShowError(result.mesg);
                }
            });
        }

        private void AddTopForumPost(ForumPost forumPost)
        {
            try
            {
                var res = PdtV1.TopForum(forumPost.ID);
                if (res.isSuccess)
                {
                    Refresh();
                }
                else
                {
                    throw new Exception(res.mesg);
                }
            }
            catch (Exception e)
            {
                MessageBoxHelper.ShowError(e.Message);
            }
        }

        private void CancelTopForumPost(ForumPost forumPost)
        {
            try
            {
                var res = PdtV1.UnTopForum(forumPost.ID);
                if (res.isSuccess)
                {
                    Refresh();
                }
                else
                {
                    throw new Exception(res.mesg);
                }
            }
            catch (Exception e)
            {
                MessageBoxHelper.ShowError(e.Message);
            }
        }

        public ForumToppingManagerViewModel()
        {
            ForumPosts = new ObservableCollection<ForumPost>();
            TopForumPosts = new ObservableCollection<ForumPost>();

            RefreshCmd = new RelayCommand(Refresh);
            LoadMorePostsCmd = new RelayCommand(LoadMorePosts);
            AddTopForumPostCmd = new RelayCommand<ForumPost>(AddTopForumPost);
            CancelTopForumPostCmd = new RelayCommand<ForumPost>(CancelTopForumPost);

            if (GlobalData.AdminMode)
            {
                Refresh();
            }
        }
    }
}
