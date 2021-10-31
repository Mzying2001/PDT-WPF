using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.PageViewModels
{
    public class AdminPageViewModel : ViewModelBase
    {
        public RelayCommand AdminLoginCmd { get; set; }
        public RelayCommand AdminLogoutCmd { get; set; }
        public RelayCommand<string> GetVerificationCodeCmd { get; set; }
        public RelayCommand<string> OpenLinkCmd { get; set; }
        public RelayCommand LoadBoardPhotosCmd { get; set; }
        public RelayCommand<BoardPhoto> DeleteBoardPhotoCmd { get; set; }
        public RelayCommand AddBoardPhotoCmd { get; set; }
        public RelayCommand LoadCompetitionSectionsCmd { get; set; }
        public RelayCommand<CompetitionSection> DeleteCompetitionSectionCmd { get; set; }



        public ObservableCollection<BoardPhoto> BoardPhotos { get; set; }
        public ObservableCollection<CompetitionSection> CompetitionSections { get; set; }



        private string _adminAccount;
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string AdminAccount
        {
            get => _adminAccount;
            set => Set(ref _adminAccount, value);
        }

        private string _adminPassword;
        /// <summary>
        /// 管理员密码
        /// </summary>
        public string AdminPassword
        {
            get => _adminPassword;
            set => Set(ref _adminPassword, value);
        }

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
                AdminLoginCmd.RaiseCanExecuteChanged();
            }
        }

        private bool _isLoggingIn;
        /// <summary>
        /// 是否正在登录
        /// </summary>
        public bool IsLoggingIn
        {
            get => _isLoggingIn;
            set => Set(ref _isLoggingIn, value);
        }

        private bool _isGettingVerificationCode;
        /// <summary>
        /// 是否正在获取验证码
        /// </summary>
        public bool IsGettingVerificationCode
        {
            get => _isGettingVerificationCode;
            set => Set(ref _isGettingVerificationCode, value);
        }

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
        /// 登录管理员账号的异步方法
        /// </summary>
        /// <param name="callback"></param>
        private async void AdminLoginAsync(Action<PdtV1.AdministratorLoginResponse> callback)
        {
            try
            {
                IsLoggingIn = true;
                callback(await Task.Run(() => PdtV1.AdministratorLogin(AdminAccount, AdminPassword)));
            }
            catch (Exception e)
            {
                callback(new PdtV1.AdministratorLoginResponse { mesg = e.Message });
            }
            finally
            {
                IsLoggingIn = false;
            }
        }

        /// <summary>
        /// 登录管理员账号
        /// </summary>
        private void AdminLogin()
        {
            AdminLoginAsync(result =>
            {
                if (result.code == Services.Http.HttpStatus.OK)
                {
                    AdminMode = true;
                    PdtCommon.AdminApiToken = result.token;

                    //处理以“-Admin”参数启动时Token为空的情况
                    if (string.IsNullOrEmpty(PdtCommon.Token))
                        PdtCommon.Token = result.token;

                    LoadBoardPhotos(); //获取首页轮播图信息
                    LoadCompetitionSections(); //获取比赛信息

                    MessageBoxHelper.ShowMessage(result.mesg, "登录成功");
                }
                else
                {
                    MessageBoxHelper.ShowError(result.mesg, "登录失败");
                }
            });
        }

        /// <summary>
        /// 退出管理员账号
        /// </summary>
        private void AdminLogout()
        {
            if (MessageBoxHelper.ShowQuestion($"是否退出管理员账号“{AdminAccount}”？"))
            {
                AdminMode = false;
                PdtCommon.AdminApiToken = string.Empty;
            }
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
                    RaisePropertyChanged("BoardPhotos");
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
                MessageBoxHelper.ShowMessage("该功能未完成。");
            }
        }

        /// <summary>
        /// 添加轮播图
        /// </summary>
        private void AddBoardPhoto()
        {

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
                    RaisePropertyChanged("CompetitionSections");
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
                MessageBoxHelper.ShowMessage("该功能未完成");
            }
        }



        public AdminPageViewModel()
        {
            AdminLoginCmd = new RelayCommand(AdminLogin, () => !AdminMode);
            AdminLogoutCmd = new RelayCommand(AdminLogout);
            GetVerificationCodeCmd = new RelayCommand<string>(GetVerificationCode);
            OpenLinkCmd = new RelayCommand<string>(OpenLink);
            LoadBoardPhotosCmd = new RelayCommand(LoadBoardPhotos, () => !IsLoadingBoardPhotos);
            DeleteBoardPhotoCmd = new RelayCommand<BoardPhoto>(DeleteBoardPhoto);
            AddBoardPhotoCmd = new RelayCommand(AddBoardPhoto);
            LoadCompetitionSectionsCmd = new RelayCommand(LoadCompetitionSections, () => !IsLoadingCompetitionSections);
            DeleteCompetitionSectionCmd = new RelayCommand<CompetitionSection>(DeleteCompetitionSection);
        }
    }
}
