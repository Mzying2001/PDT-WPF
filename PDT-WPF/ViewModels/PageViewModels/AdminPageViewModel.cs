using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDT_WPF.Models.Data;
using PDT_WPF.Services.Api;
using PDT_WPF.Views.Utils;
using System;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels.PageViewModels
{
    public class AdminPageViewModel : ViewModelBase
    {
        public RelayCommand AdminLoginCmd { get; set; }
        public RelayCommand AdminLogoutCmd { get; set; }



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
            }
        }



        public AdminPageViewModel()
        {
            AdminLoginCmd = new RelayCommand(AdminLogin, () => !AdminMode);
            AdminLogoutCmd = new RelayCommand(AdminLogout);
        }
    }
}
