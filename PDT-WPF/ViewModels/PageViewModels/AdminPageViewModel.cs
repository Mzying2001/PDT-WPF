using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Utils;
using PDT_WPF.Views.Dialogs;
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

        public RelayCommand LoadProjectMatchesCmd { get; set; }
        public RelayCommand AddProjectMatchCmd { get; set; }
        public RelayCommand<ProjectMatch> DeleteProjectMatchCmd { get; set; }
        public RelayCommand<ProjectMatch> ChangeProjectMatchCmd { get; set; }

        public RelayCommand LoadPersonnelTechnologyTagsCmd { get; set; }
        public RelayCommand AddPersonnelTechnologyTagCmd { get; set; }
        public RelayCommand<TechnologyTagItem> DeletePersonnelTechnologyTagCmd { get; set; }
        public RelayCommand<TechnologyTagItem> ChangePersonnelTechnologyTagCmd { get; set; }

        public RelayCommand AddAdministratorCmd { get; set; }



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

        #region 项目赛事标签管理

        public ObservableCollection<ProjectMatch> ProjectMatches { get; set; }

        private bool _isLoadingProjectMatches;
        /// <summary>
        /// 是否正在加载项目赛事标签
        /// </summary>
        public bool IsLoadingProjectMatches
        {
            get => _isLoadingProjectMatches;
            set
            {
                _isLoadingProjectMatches = value;
                LoadProjectMatchesCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 加载项目赛事标签的异步方法
        /// </summary>
        /// <param name="callback"></param>
        private async void LoadProjectMatchesAsync(Action<PdtV1.GetProjectMatchResponse> callback)
        {
            try
            {
                IsLoadingProjectMatches = true;
                callback(await Task.Run(() => PdtV1.GetProjectMatch()));
            }
            catch
            {
                callback(default);
            }
            finally
            {
                IsLoadingProjectMatches = false;
            }
        }

        /// <summary>
        /// 加载项目赛事标签
        /// </summary>
        private void LoadProjectMatches()
        {
            if (ProjectMatches == null)
                ProjectMatches = new ObservableCollection<ProjectMatch>();

            LoadProjectMatchesAsync(result =>
            {
                if (result.code == Services.Http.HttpStatus.OK)
                {
                    ProjectMatches.Clear();
                    foreach (var item in result.projectMatch)
                        ProjectMatches.Add(item);
                    RaisePropertyChanged(nameof(ProjectMatches));
                }
                else
                {
                    MessageBoxHelper.ShowError("获取项目赛事标签失败！");
                }
            });
        }

        /// <summary>
        /// 添加项目赛事标签
        /// </summary>
        private void AddProjectMatch()
        {
            InputStringDialog.ShowDialog((success, input) =>
            {
                if (success)
                {
                    try
                    {
                        var res = PdtV1.AddProjectMatch(input);
                        if (res.code == Services.Http.HttpStatus.OK)
                        {
                            LoadProjectMatches();
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
            }, "请输入新的项目赛事");
        }

        /// <summary>
        /// 删除项目赛事标签
        /// </summary>
        /// <param name="projectMatch"></param>
        private void DeleteProjectMatch(ProjectMatch projectMatch)
        {
            if (MessageBoxHelper.ShowQuestion($"确定要删除“{projectMatch.MatchName}”吗？"))
            {
                try
                {
                    var res = PdtV1.DeleteProjectMatch(projectMatch.MatchId);
                    if (res.code == Services.Http.HttpStatus.OK)
                    {
                        LoadProjectMatches();
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
        /// 修改项目赛事标签
        /// </summary>
        /// <param name="projectMatch"></param>
        private void ChangeProjectMatch(ProjectMatch projectMatch)
        {
            InputStringDialog.ShowDialog((success, input) =>
            {
                if (success)
                {
                    try
                    {
                        var res = PdtV1.ChangeProjectMatch(projectMatch.MatchId, input);
                        if (res.code == Services.Http.HttpStatus.OK)
                        {
                            LoadProjectMatches();
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
            }, "修改项目赛事", projectMatch.MatchName);
        }

        #endregion

        #region 人才技能标签管理

        public ObservableCollection<TechnologyTagItem> TechnologyTags { get; set; }

        private bool _isLoadingTechnologyTags;
        /// <summary>
        /// 是否正在加载人才技能标签
        /// </summary>
        public bool IsLoadingTechnologyTags
        {
            get => _isLoadingTechnologyTags;
            set
            {
                _isLoadingTechnologyTags = value;
                LoadPersonnelTechnologyTagsCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 加载人才技能标签的异步方法
        /// </summary>
        /// <param name="callback"></param>
        private async void LoadTechnologyTagsAsync(Action<PdtV1.GetPersonnelTechnologyTagsResponse> callback)
        {
            try
            {
                IsLoadingTechnologyTags = true;
                callback(await Task.Run(() => PdtV1.GetPersonnelTechnologyTags()));
            }
            catch
            {
                callback(default);
            }
            finally
            {
                IsLoadingTechnologyTags = false;
            }
        }

        /// <summary>
        /// 加载人才技能标签
        /// </summary>
        private void LoadTechnologyTags()
        {
            if (TechnologyTags == null)
                TechnologyTags = new ObservableCollection<TechnologyTagItem>();

            LoadTechnologyTagsAsync(result =>
            {
                if (result.code == Services.Http.HttpStatus.OK)
                {
                    TechnologyTags.Clear();
                    foreach (var item in result.mainTechnologys)
                        TechnologyTags.Add(item);
                    RaisePropertyChanged(nameof(TechnologyTags));
                }
                else
                {
                    MessageBoxHelper.ShowError("获取人才技能标签失败！");
                }
            });
        }

        /// <summary>
        /// 添加人才技能标签
        /// </summary>
        private void AddTechnologyTag()
        {
            InputStringDialog.ShowDialog((success, input) =>
            {
                if (success)
                {
                    try
                    {
                        var res = PdtV1.AddPersonnelTechnologyTag(input);
                        if (res.code == Services.Http.HttpStatus.OK)
                        {
                            LoadTechnologyTags();
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
            }, "请输入新的技能标签");
        }

        /// <summary>
        /// 删除人才技能标签
        /// </summary>
        /// <param name="technologyTag"></param>
        private void DeleteTechnologyTag(TechnologyTagItem technologyTag)
        {
            if (MessageBoxHelper.ShowQuestion($"确定要删除“{technologyTag.TechnologyTag}”吗？"))
            {
                try
                {
                    var res = PdtV1.DeletePersonnelTechnologyTag(technologyTag.TechnologyTagId);
                    if (res.code == Services.Http.HttpStatus.OK)
                    {
                        LoadTechnologyTags();
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
        /// 修改人才技能标签
        /// </summary>
        /// <param name="technologyTag"></param>
        private void ChangeTechnologyTag(TechnologyTagItem technologyTag)
        {
            InputStringDialog.ShowDialog((success, input) =>
            {
                if (success)
                {
                    try
                    {
                        var res = PdtV1.ChangePersonnelTechnologyTag(technologyTag.TechnologyTagId, input);
                        if (res.code == Services.Http.HttpStatus.OK)
                        {
                            LoadTechnologyTags();
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
            }, "修改技能标签", technologyTag.TechnologyTag);
        }

        #endregion

        #region 后台管理账号

        private string _newAdminAccountName;
        /// <summary>
        /// 新管理员账号名称
        /// </summary>
        public string NewAdminAccountName
        {
            get => _newAdminAccountName;
            set => Set(ref _newAdminAccountName, value);
        }

        private string _newAdminAccountPassword;
        /// <summary>
        /// 新管理员账号密码
        /// </summary>
        public string NewAdminAccountPassword
        {
            get => _newAdminAccountPassword;
            set => Set(ref _newAdminAccountPassword, value);
        }

        private string _newAdminAccountPasswordRepeat;
        /// <summary>
        /// 用于验证新管理员账号密码
        /// </summary>
        public string NewAdminAccountPasswordRepeat
        {
            get => _newAdminAccountPasswordRepeat;
            set => Set(ref _newAdminAccountPasswordRepeat, value);
        }

        private string _newAdminAccountDescription;
        /// <summary>
        /// 新管理员账号描述
        /// </summary>
        public string NewAdminAccountDescription
        {
            get => _newAdminAccountDescription;
            set => Set(ref _newAdminAccountDescription, value);
        }

        private bool _onAddAdminAccount;
        /// <summary>
        /// 是否正在添加管理员账户
        /// </summary>
        public bool OnAddAdminAccount
        {
            get => _onAddAdminAccount;
            set
            {
                Set(ref _onAddAdminAccount, value);
                AddAdministratorCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 添加管理员账户的异步方法
        /// </summary>
        private async void AddAdministratorAsync(Action<PdtV1.AddAdministratorResponse> callback)
        {
            try
            {
                OnAddAdminAccount = true;
                callback(await Task.Run(() =>
                    PdtV1.AddAdministrator(NewAdminAccountName, NewAdminAccountPassword, NewAdminAccountDescription)));
            }
            catch
            {
                callback(default);
            }
            finally
            {
                OnAddAdminAccount = false;
            }
        }

        /// <summary>
        /// 添加管理员账户
        /// </summary>
        private void AddAdministrator()
        {
            if (NewAdminAccountPassword == NewAdminAccountPasswordRepeat)
            {
                AddAdministratorAsync(result =>
                {
                    if (result.code == Services.Http.HttpStatus.OK)
                    {
                        MessageBoxHelper.ShowMessage(result.mesg);
                    }
                    else
                    {
                        MessageBoxHelper.ShowError(result.mesg);
                    }
                });
            }
            else
            {
                MessageBoxHelper.ShowMessage("两次输入的密码不一致");
            }
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

            LoadProjectMatchesCmd = new RelayCommand(LoadProjectMatches, () => !IsLoadingProjectMatches);
            AddProjectMatchCmd = new RelayCommand(AddProjectMatch);
            DeleteProjectMatchCmd = new RelayCommand<ProjectMatch>(DeleteProjectMatch);
            ChangeProjectMatchCmd = new RelayCommand<ProjectMatch>(ChangeProjectMatch);

            LoadPersonnelTechnologyTagsCmd = new RelayCommand(LoadTechnologyTags, () => !IsLoadingTechnologyTags);
            AddPersonnelTechnologyTagCmd = new RelayCommand(AddTechnologyTag);
            DeletePersonnelTechnologyTagCmd = new RelayCommand<TechnologyTagItem>(DeleteTechnologyTag);
            ChangePersonnelTechnologyTagCmd = new RelayCommand<TechnologyTagItem>(ChangeTechnologyTag);

            AddAdministratorCmd = new RelayCommand(AddAdministrator, () => !OnAddAdminAccount);


            if (GlobalData.AdminMode)
            {
                LoadBoardPhotos();
                LoadCompetitionSections();
                LoadProjectMainTechnologies();
                LoadProjectTypes();
                LoadProjectMatches();
                LoadTechnologyTags();
            }
        }
    }
}
