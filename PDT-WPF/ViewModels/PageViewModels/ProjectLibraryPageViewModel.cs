using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Models;
using PDT_WPF.Services.Api;
using PDT_WPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PDT_WPF.ViewModels.PageViewModels
{
    public class ProjectLibraryPageViewModel : ViewModelBase
    {
        public ObservableCollection<Project> Projects { get; set; }

        public RelayCommand LoadMoreProjectsCmd { get; set; }
        public RelayCommand ReloadProjectListCmd { get; set; }
        public RelayCommand<Project> ShowProjectCmd { get; set; }

        private int Page { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    Set(ref _isLoading, value);
                    LoadMoreProjectsCmd.RaiseCanExecuteChanged();
                    ReloadProjectListCmd.RaiseCanExecuteChanged();
                }
            }
        }

        private async void GetProjectListAsync(Action<PdtV1.GetProjectListResponse> callback)
        {
            IsLoading = true;
            try
            {
                var res = await Task.Run(() => PdtV1.GetProjectList((++Page).ToString(), "0"));
                if (!res.isSuccess)
                    Page--;
                callback(res);
            }
            catch (Exception e)
            {
                Page--;
                callback(new PdtV1.GetProjectListResponse
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

        private void InitProjectList()
        {
            Projects = new ObservableCollection<Project>();
            LoadMoreProjects();
        }

        private void LoadMoreProjects()
        {
            //MessageBoxHelper.ShowMessage(Page.ToString());
            GetProjectListAsync(result =>
            {
                if (result.isSuccess)
                {
                    foreach (var item in result.projectList)
                        Projects.Add(item);
                }
                else
                {
                    MessageBoxHelper.ShowError(result.mesg);
                }
            });
        }

        private void ReloadProjectList()
        {
            int tmp = Page;
            Page = 0;
            GetProjectListAsync(result =>
            {
                if (result.isSuccess)
                {
                    Projects.Clear();
                    foreach (var item in result.projectList)
                        Projects.Add(item);
                }
                else
                {
                    Page = tmp;
                    MessageBoxHelper.ShowError(result.mesg);
                }
            });
        }

        private void ShowProject(Project project)
        {
            MessageBoxHelper.ShowMessage(Newtonsoft.Json.JsonConvert.SerializeObject(project));
        }

        public ProjectLibraryPageViewModel()
        {
            LoadMoreProjectsCmd = new RelayCommand(LoadMoreProjects, () => !IsLoading);
            ReloadProjectListCmd = new RelayCommand(ReloadProjectList, () => !IsLoading);
            ShowProjectCmd = new RelayCommand<Project>(ShowProject);


            InitProjectList();
        }
    }
}
