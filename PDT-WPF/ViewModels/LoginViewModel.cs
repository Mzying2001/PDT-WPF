using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Models.Data;
using PDT_WPF.Services;
using PDT_WPF.Services.Api;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public RelayCommand LoginCmd { get; set; }
        public RelayCommand AdminLoginCmd { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private string _account = "";
        public string Account
        {
            get => _account;
            set => Set(ref _account, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        /// <summary>
        /// 启动时使用本地保存的OpenId自动登录
        /// </summary>
        private async void AutoLoginAsync()
        {
            if (string.IsNullOrEmpty(LocalData.Settings.OpenId))
                return;

            try
            {
                IsLoading = true;

                var loginRes = await Task.Run(() => PdtV1.Login(LocalData.Settings.OpenId));
                if (loginRes.code == Http.HttpStatus.OK)
                {
                    PdtCommon.Token = loginRes.token;
                }
                else
                {
                    return;
                }

                var res = await Task.Run(() => PdtV1.GetUserInfo(LocalData.Settings.OpenId));
                if (res.code == Http.HttpStatus.OK)
                {
                    GlobalData.CurrentUser = res.user;
                    Messenger.Default.Send(new LoginResult { Success = true }, MessageTokens.LOGIN_RESULT);
                }
            }
            catch
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void LoginAsync(Action<LoginResult> callback)
        {
            try
            {
                IsLoading = true;

                //开始登录
                PdtV1.LoginResponse loginRes;
                if (Regex.IsMatch(Account, "^OPEN_ID{.+}$"))
                {
                    string openId = Account.Substring(8, Account.Length - 9);
                    loginRes = await Task.Run(() => PdtV1.Login(openId));

                    if (loginRes.code == Http.HttpStatus.OK)
                        LocalData.Settings.OpenId = openId;
                }
                else
                {
                    //await Task.Run(() => System.Threading.Thread.Sleep(3000));
                    throw new Exception("后端没给接口啊~");
                }


                if (loginRes.code == Http.HttpStatus.OK)
                {
                    //登录成功，保存token
                    PdtCommon.Token = loginRes.token;
                }
                else
                {
                    //登陆失败，抛出错误
                    throw new Exception(loginRes.mesg);
                }


                //登录成功，开始获取用户信息
                var res = await Task.Run(() => PdtV1.GetUserInfo(LocalData.Settings.OpenId));
                if (res.code == Http.HttpStatus.OK)
                {
                    //获取用户信息成功，保存用户信息
                    GlobalData.CurrentUser = res.user;
                }

                callback(new LoginResult
                {
                    Success = res.code == Http.HttpStatus.OK,
                    Message = res.mesg
                });
            }
            catch (Exception e)
            {
                callback(new LoginResult
                {
                    Success = false,
                    Message = e.Message
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Login()
        {
            LoginAsync(result =>
            {
                GlobalData.IsLogined = result.Success;
                Messenger.Default.Send(result, MessageTokens.LOGIN_RESULT);
            });
        }

        private async void AdminLoginAsync(Action<LoginResult> callback)
        {
            try
            {
                IsLoading = true;

                var loginRes = await Task.Run(() => PdtV1.AdministratorLogin(Account, Password));

                if (loginRes.code == Http.HttpStatus.OK)
                {
                    //登录成功
                    PdtCommon.Token = loginRes.token;
                }
                else
                {
                    //登录失败
                    throw new Exception(loginRes.mesg);
                }

                GlobalData.AdminMode = true;
                GlobalData.CurrentUser = new User
                {
                    NickName = $"管理员: {Account}",
                    Avaurl = "/Assets/Images/DefaultAvatar.png"
                };

                callback(new LoginResult
                {
                    Success = true,
                    Message = loginRes.mesg
                });
            }
            catch (Exception e)
            {
                callback(new LoginResult
                {
                    Success = false,
                    Message = e.Message
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AdminLogin()
        {
            AdminLoginAsync(result =>
            {
                GlobalData.IsLogined = result.Success;
                Messenger.Default.Send(result, MessageTokens.LOGIN_RESULT);
            });
        }

        public LoginViewModel()
        {
            LoginCmd = new RelayCommand(Login);
            AdminLoginCmd = new RelayCommand(AdminLogin);

            AutoLoginAsync();
        }
    }
}
