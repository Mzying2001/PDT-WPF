using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Dialogs;
using PDT_WPF.Views.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.PageViewModels
{
    public class AdminPageViewModel : ViewModelBase
    {
        public RelayCommand<string> GetVerificationCodeCmd { get; set; }
        public RelayCommand<string> OpenLinkCmd { get; set; }

        public RelayCommand LoadBoardPhotosCmd { get; set; }
        public RelayCommand<BoardPhoto> DeleteBoardPhotoCmd { get; set; }
        public RelayCommand AddBoardPhotoCmd { get; set; }

        public RelayCommand LoadCompetitionSectionsCmd { get; set; }
        public RelayCommand<CompetitionSection> DeleteCompetitionSectionCmd { get; set; }
        public RelayCommand AddCompetitionSectionCmd { get; set; }

        public RelayCommand LoadProjectMainTechnologiesCmd { get; set; }
        public RelayCommand AddProjectMainTechnologyCmd { get; set; }
        public RelayCommand<ProjectMainTechnology> DeleteProjectMainTechnologyCmd { get; set; }
        public RelayCommand<ProjectMainTechnology> ChangeProjectMainTechnologyCmd { get; set; }

        public RelayCommand LoadProjectTypesCmd { get; set; }
        public RelayCommand AddProjectTypeCmd { get; set; }
        public RelayCommand<ProjectTypeItem> DeleteProjectTypeCmd { get; set; }
        public RelayCommand<ProjectTypeItem> ChangeProjectTypeCmd { get; set; }



        /// <summary>
        /// 是否处于管理员模式
        /// </summary>
        public bool AdminMode
        {
            get => GlobalData.AdminMode;
            set
            {
                GlobalData.AdminMode = value;
                RaisePropertyChanged();
                //AdminLoginCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 通知主页更新数据
        /// </summary>
        private void NotifyHomepageDataUpdated()
        {
            Messenger.Default.Send<object>(null, MessageTokens.HOMEPAGE_DATA_UPDATED);
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="link"></param>
        private void OpenLink(string link)
        {
            try
            {
                Process.Start(link);
            }
            catch (Exception e)
            {
                MessageBoxHelper.ShowError(e);
            }
        }



        #region 人工验证获取验证码

        private bool _isGettingVerificationCode;
        /// <summary>
        /// 是否正在获取验证码
        /// </summary>
        public bool IsGettingVerificationCode
        {
            get => _isGettingVerificationCode;
            set => Set(ref _isGettingVerificationCode, value);
        }

        /// <summary>
        /// 人工验证获取验证码的异步方法
        /// </summary>
        /// <param name="callback"></param>
        private async void GetVerificationCodeAsync(string schoolId, Action<PdtV1.GetVerificationCodeResponse> callback)
        {
            try
            {
                IsGettingVerificationCode = true;
                callback(await Task.Run(() => PdtV1.GetVerificationCode(schoolId)));
            }
            catch (Exception e)
            {
                callback(new PdtV1.GetVerificationCodeResponse
                {
                    isSuccess = false,
                    mesg = e.Message
                });
            }
            finally
            {
                IsGettingVerificationCode = false;
            }
        }

        /// <summary>
        /// 人工验证获取验证码
        /// </summary>
        private void GetVerificationCode(string schoolId)
        {
            GetVerificationCodeAsync(schoolId, result =>
            {
                if (result.isSuccess)
                {
                    MessageBoxHelper.ShowMessage($"验证码：{result.code}", "获取成功");
                }
                else
                {
                    MessageBoxHelper.ShowError(result.mesg, "获取失败");
                }
            });
        }

        #endregion

        #region 首页轮播图管理

        public ObservableCollection<BoardPhoto> BoardPhotos { get; set; }

        private bool _isLoadingBoardPhotos;
        /// <summary>
        /// 是否正在获取首页轮播图信息
        /// </summary>
        public bool IsLoadingBoardPhotos
        {
            get => _isLoadingBoardPhotos;
            set
            {
                Set(ref _isLoadingBoardPhotos, value);
                LoadBoardPhotosCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 获取首页轮播图的异步方法
        /// </summary>
        /// <param name="callback"></param>
        private async void GetBoardPhotosAsync(Action<BoardPhoto[]> callback)
        {
            try
            {
                IsLoadingBoardPhotos = true;
                callback(await Task.Run(() => PdtV2.GetBoardPhotos()));
            }
            catch
            {
                callback(null);
            }
            finally
            {
                IsLoadingBoardPhotos = false;
            }
        }

        /// <summary>
        /// 获取首页轮播图
        /// </summary>
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
                    RaisePropertyChanged(nameof(BoardPhotos));
                }
                else
                {
                    MessageBoxHelper.ShowError("获取首页轮播图信息失败！");
                }
            });
        }

        /// <summary>
        /// 移除轮播图
        /// </summary>
        /// <param name="id"></param>
        private void DeleteBoardPhoto(BoardPhoto boardPhoto)
        {
            if (MessageBoxHelper.ShowQuestion($"确定要删除“{boardPhoto.Name}”吗？"))
            {
                try
                {
                    var res = PdtV2.DeleteBoardPhoto(boardPhoto.ID);
                    if (res.code == Services.Http.HttpStatus.OK)
                    {
                        //MessageBoxHelper.ShowMessage(res.mesg, "删除成功");
                        BoardPhotos.Remove(boardPhoto);
                        NotifyHomepageDataUpdated();
                    }
                    else
                    {
                        throw new Exception(res.message);
                    }
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e, "删除失败");
                }
            }
        }

        /// <summary>
        /// 添加轮播图
        /// </summary>
        private void AddBoardPhoto()
        {
            AddBoardPhotoDialog.ShowDialog(result =>
            {
                if (result)
                {
                    LoadBoardPhotos();
                    NotifyHomepageDataUpdated();
                }
            });
        }

        #endregion

        #region 比赛信息管理

        public ObservableCollection<CompetitionSection> CompetitionSections { get; set; }

        private bool _isLoadingCompetitionSections;
        /// <summary>
        /// 是否正在获取比赛信息
        /// </summary>
        public bool IsLoadingCompetitionSections
        {
            get => _isLoadingCompetitionSections;
            set
            {
                Set(ref _isLoadingCompetitionSections, value);
                LoadCompetitionSectionsCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 获取比赛信息的异步方法
        /// </summary>
        /// <param name="callback"></param>
        private async void GetCompetitionSectionsAsync(Action<CompetitionSection[][]> callback)
        {
            try
            {
                IsLoadingCompetitionSections = true;
                callback(await Task.Run(() => PdtV2.GetCompetitionSections()));
            }
            catch
            {
                callback(null);
            }
            finally
            {
                IsLoadingCompetitionSections = false;
            }
        }

        /// <summary>
        /// 获取比赛信息
        /// </summary>
        private void LoadCompetitionSections()
        {
            if (CompetitionSections == null)
                CompetitionSections = new ObservableCollection<CompetitionSection>();

            GetCompetitionSectionsAsync(result =>
            {
                if (result != null)
                {
                    CompetitionSections.Clear();
                    foreach (var arr in result)
                    {
                        foreach (var item in arr)
                            CompetitionSections.Add(item);
                    }
                    RaisePropertyChanged(nameof(CompetitionSections));
                }
                else
                {
                    MessageBoxHelper.ShowError("获取比赛信息失败！");
                }
            });
        }

        /// <summary>
        /// 删除比赛信息
        /// </summary>
        /// <param name="competitionSection"></param>
        private void DeleteCompetitionSection(CompetitionSection competitionSection)
        {
            if (MessageBoxHelper.ShowQuestion($"确定要删除“{competitionSection.Title}”吗？"))
            {
                try
                {
                    var res = PdtV2.DeleteCompetitionSection(competitionSection.ID);
                    if (res.code == Services.Http.HttpStatus.OK)
                    {
                        //MessageBoxHelper.ShowMessage(res.mesg, "删除成功");
                        CompetitionSections.Remove(competitionSection);
                        NotifyHomepageDataUpdated();
                    }
                    else
                    {
                        throw new Exception(res.message);
                    }
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e, "删除失败");
                }
            }
        }

        /// <summary>
        /// 添加比赛栏信息
        /// </summary>
        private void AddCompetitionSection()
        {
            AddCompetitionSectionDialoog.ShowDialog(result =>
            {
                if (result)
                {
                    LoadCompetitionSections();
                    NotifyHomepageDataUpdated();
                }
            });
        }

        #endregion

        #region 项目主要技术标签管理

        public ObservableCollection<ProjectMainTechnology> ProjectMainTechnologies { get; set; }

        private bool _isLoadingProjectMainTechnologies;
        /// <summary>
        /// 是否正在加载项目主要技术标签
        /// </summary>
        public bool IsLoadingProjectMainTechnologies
        {
            get => _isLoadingProjectMainTechnologies;
            set
            {
                _isLoadingProjectMainTechnologies = value;
                LoadProjectMainTechnologiesCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 加载项目主要技术标签的异步方法
        /// </summary>
        /// <param name="callback"></param>
        private async void LoadProjectMainTechnologiesAsync(Action<PdtV1.GetProjectMainTechnologyResponse> callback)
        {
            try
            {
                IsLoadingProjectMainTechnologies = true;
                callback(await Task.Run(() => PdtV1.GetProjectMainTechnology()));
            }
            catch
            {
                callback(default);
            }
            finally
            {
                IsLoadingProjectMainTechnologies = false;
            }
        }

        /// <summary>
        /// 加载项目主要技术标签
        /// </summary>
        private void LoadProjectMainTechnologies()
        {
            if (ProjectMainTechnologies == null)
                ProjectMainTechnologies = new ObservableCollection<ProjectMainTechnology>();

            LoadProjectMainTechnologiesAsync(result =>
            {
                if (result.code == Services.Http.HttpStatus.OK)
                {
                    ProjectMainTechnologies.Clear();
                    foreach (var item in result.mainTechnologys)
                        ProjectMainTechnologies.Add(item);
                    RaisePropertyChanged(nameof(ProjectMainTechnologies));
                }
                else
                {
                    MessageBoxHelper.ShowError("获取项目主要技术标签失败！");
                }
            });
        }

        /// <summary>
        /// 添加项目主要技术标签
        /// </summary>
        private void AddProjectMainTechnology()
        {
            InputStringDialog.ShowDialog((success, input) =>
            {
                if (success)
                {
                    try
                    {
                        var res = PdtV1.AddProjectMainTechnology(input);
                        if (res.code == Services.Http.HttpStatus.OK)
                        {
                            LoadProjectMainTechnologies();
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
            }, "请输入新的技术标签");
        }

        /// <summary>
        /// 删除项目主要技术标签
        /// </summary>
        /// <param name="mainTechnology"></param>
        private void DeleteProjectMainTechnology(ProjectMainTechnology mainTechnology)
        {
            if (MessageBoxHelper.ShowQuestion($"确定要删除“{mainTechnology.MainTechnology}”吗？"))
            {
                try
                {
                    var res = PdtV1.DeleteProjectMainTechnology(mainTechnology.ID);
                    if (res.code == Services.Http.HttpStatus.OK)
                    {
                        LoadProjectMainTechnologies();
                    }
                    else
                    {
                        throw new Exception(res.mesg);
                    }
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e, "删除失败");
                }
            }
        }

        /// <summary>
        /// 修改项目主要技术标签
        /// </summary>
        /// <param name="mainTechnology"></param>
        private void ChangeProjectMainTechnology(ProjectMainTechnology mainTechnology)
        {
            InputStringDialog.ShowDialog((success, input) =>
            {
                if (success)
                {
                    try
                    {
                        var res = PdtV1.ChangeProjectMainTechnology(mainTechnology.ID, input);
                        if (res.code == Services.Http.HttpStatus.OK)
                        {
                            LoadProjectMainTechnologies();
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
            }, "修改技术标签", mainTechnology.MainTechnology);
        }

        #endregion

        #region 项目类型标签管理

        public ObservableCollection<ProjectTypeItem> ProjectTypes { get; set; }

        private bool _isLoadingProjectTypes;
        /// <summary>
        /// 是否正在加载项目类型标签
        /// </summary>
        public bool IsLoadingProjectTypes
        {
            get => _isLoadingProjectTypes;
            set
            {
                _isLoadingProjectTypes = value;
                LoadProjectTypesCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 加载项目类型标签的异步方法
        /// </summary>
        /// <param name="callback"></param>
        private async void LoadProjectTypesAsync(Action<PdtV1.GetProjectTypeResponse> callback)
        {
            try
            {
                IsLoadingProjectTypes = true;
                callback(await Task.Run(() => PdtV1.GetProjectType()));
            }
            catch
            {
                callback(default);
            }
            finally
            {
                IsLoadingProjectTypes = false;
            }
        }

        /// <summary>
        /// 加载项目类型标签
        /// </summary>
        private void LoadProjectTypes()
        {
            if (ProjectTypes == null)
                ProjectTypes = new ObservableCollection<ProjectTypeItem>();

            LoadProjectTypesAsync(result =>
            {
                if (result.code == Services.Http.HttpStatus.OK)
                {
                    ProjectTypes.Clear();
                    foreach (var item in result.projectTypes)
                        ProjectTypes.Add(item);
                    RaisePropertyChanged(nameof(ProjectTypes));
                }
                else
                {
                    MessageBoxHelper.ShowError("获取项目类型标签失败！");
                }
            });
        }

        /// <summary>
        /// 添加项目类型标签
        /// </summary>
        private void AddProjectType()
        {
            InputStringDialog.ShowDialog((success, input) =>
            {
                if (success)
                {
                    try
                    {
                        var res = PdtV1.AddProjectType(input);
                        if (res.code == Services.Http.HttpStatus.OK)
                        {
                            LoadProjectTypes();
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
            }, "请输入新的项目类型标签");
        }

        /// <summary>
        /// 删除项目类型标签
        /// </summary>
        /// <param name="projectType"></param>
        private void DeleteProjectType(ProjectTypeItem projectType)
        {
            if (MessageBoxHelper.ShowQuestion($"确定要删除“{projectType.ProjectType}”吗？"))
            {
                try
                {
                    var res = PdtV1.DeleteProjectType(projectType.ID);
                    if (res.code == Services.Http.HttpStatus.OK)
                    {
                        LoadProjectTypes();
                    }
                    else
                    {
                        throw new Exception(res.mesg);
                    }
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e, "删除失败");
                }
            }
        }

        /// <summary>
        /// 修改项目类型标签
        /// </summary>
        /// <param name="projectType"></param>
        private void ChangeProjectType(ProjectTypeItem projectType)
        {
            InputStringDialog.ShowDialog((success, input) =>
            {
                if (success)
                {
                    try
                    {
                        var res = PdtV1.ChangeProjectType(projectType.ID, input);
                        if (res.code == Services.Http.HttpStatus.OK)
                        {
                            LoadProjectTypes();
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
            }, "修改项目类型标签", projectType.ProjectType);
        }

        #endregion



        public AdminPageViewModel()
        {
            GetVerificationCodeCmd = new RelayCommand<string>(GetVerificationCode);
            OpenLinkCmd = new RelayCommand<string>(OpenLink);

            LoadBoardPhotosCmd = new RelayCommand(LoadBoardPhotos, () => !IsLoadingBoardPhotos);
            DeleteBoardPhotoCmd = new RelayCommand<BoardPhoto>(DeleteBoardPhoto);
            AddBoardPhotoCmd = new RelayCommand(AddBoardPhoto);

            LoadCompetitionSectionsCmd = new RelayCommand(LoadCompetitionSections, () => !IsLoadingCompetitionSections);
            DeleteCompetitionSectionCmd = new RelayCommand<CompetitionSection>(DeleteCompetitionSection);
            AddCompetitionSectionCmd = new RelayCommand(AddCompetitionSection);

            LoadProjectMainTechnologiesCmd = new RelayCommand(LoadProjectMainTechnologies, () => !IsLoadingProjectMainTechnologies);
            AddProjectMainTechnologyCmd = new RelayCommand(AddProjectMainTechnology);
            DeleteProjectMainTechnologyCmd = new RelayCommand<ProjectMainTechnology>(DeleteProjectMainTechnology);
            ChangeProjectMainTechnologyCmd = new RelayCommand<ProjectMainTechnology>(ChangeProjectMainTechnology);

            LoadProjectTypesCmd = new RelayCommand(LoadProjectTypes, () => !IsLoadingProjectTypes);
            AddProjectTypeCmd = new RelayCommand(AddProjectType);
            DeleteProjectTypeCmd = new RelayCommand<ProjectTypeItem>(DeleteProjectType);
            ChangeProjectTypeCmd = new RelayCommand<ProjectTypeItem>(ChangeProjectType);


            if (GlobalData.AdminMode)
            {
                LoadBoardPhotos();
                LoadCompetitionSections();
                LoadProjectMainTechnologies();
                LoadProjectTypes();
            }
        }
    }
}
