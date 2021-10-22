﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PDT_WPF.Models;
using PDT_WPF.Services.Api;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDT_WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public RelayCommand LoginCmd { get; set; }

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

            IsLoading = true;
            try
            {
                var loginRes = await Task.Run(() => Pdt.Login(LocalData.Settings.OpenId));
                if (loginRes.code == Pdt.LoginResponse.SUCCESS_CODE)
                {
                    Pdt.Token = loginRes.token;
                    LocalData.Settings.UserId = loginRes.userId;
                }
                else
                {
                    return;
                }

                var res = await Task.Run(() => Pdt.GetUserInfo(LocalData.Settings.OpenId));
                if (res.isSuccess)
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
            IsLoading = true;
            try
            {
                //开始登录
                Pdt.LoginResponse loginRes;
                if (Regex.IsMatch(Account, "^OPEN_ID{.+}$"))
                {
                    string openId = Account.Substring(8, Account.Length - 9);
                    loginRes = await Task.Run(() => Pdt.Login(openId));

                    if (loginRes.code == Pdt.LoginResponse.SUCCESS_CODE)
                        LocalData.Settings.OpenId = openId;
                }
                else
                {
                    //await Task.Run(() => System.Threading.Thread.Sleep(3000));
                    throw new Exception("暂不支持使用此方式登录。");
                }


                if (loginRes.code == Pdt.LoginResponse.SUCCESS_CODE)
                {
                    //登录成功，保存token
                    Pdt.Token = loginRes.token;
                }
                else
                {
                    //登陆失败，抛出错误
                    throw new Exception(loginRes.mesg);
                }


                //登录成功，开始获取用户信息
                var res = await Task.Run(() => Pdt.GetUserInfo(LocalData.Settings.OpenId));
                if (res.isSuccess)
                {
                    //获取用户信息成功，保存用户信息
                    GlobalData.CurrentUser = res.user;
                }

                callback(new LoginResult
                {
                    Success = res.isSuccess,
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
                Messenger.Default.Send(result, MessageTokens.LOGIN_RESULT);
            });
        }

        public LoginViewModel()
        {
            LoginCmd = new RelayCommand(Login);

            AutoLoginAsync();
        }
    }
}
