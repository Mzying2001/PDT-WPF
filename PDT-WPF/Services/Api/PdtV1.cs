using Newtonsoft.Json;
using PDT_WPF.Models;
using System.Collections.Generic;

using static PDT_WPF.Services.Api.PdtCommon;

namespace PDT_WPF.Services.Api
{
    public static class PdtV1
    {
        public const string BASE_URL = "https://pdt.ojbk.me/api/v1/";


        #region 用户

        public class VerfyUserResponse
        {
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 教务系统验证
        /// </summary>
        /// <param name="schoolId">学号/工号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static VerfyUserResponse VerfyUser(string schoolId, string password)
        {
            string url = BASE_URL + "user/check";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["schoolId"] = schoolId,
                ["password"] = password
            }, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<VerfyUserResponse>(res);
        }



        public struct CheckEmailCodeResponse
        {
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 验证邮箱/人工验证码
        /// </summary>
        /// <param name="schoolId">学号/工号</param>
        /// <param name="code">邮箱验证码</param>
        /// <returns></returns>
        public static CheckEmailCodeResponse CheckEmailCode(string schoolId, string code)
        {
            string url = BASE_URL + "user/checkEmailCode";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["schoolId"] = schoolId,
                ["code"] = code
            }, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<CheckEmailCodeResponse>(res);
        }



        public struct SendEmailResponse
        {
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 发送学生邮箱认证邮件
        /// </summary>
        /// <param name="schoolId">学号/工号</param>
        /// <returns></returns>
        public static SendEmailResponse SendEmail(string schoolId)
        {
            string url = BASE_URL + "user/sendEmail";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["schoolId"] = schoolId
            }, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<SendEmailResponse>(res);
        }



        public struct LoginResponse
        {
            public Http.HttpStatus code;
            public string mesg;
            public string token;
            public int userId;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="openId">用户的openid</param>
        /// <returns></returns>
        public static LoginResponse Login(string openId)
        {
            string url = BASE_URL + "user/login";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["openId"] = openId
            }, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<LoginResponse>(res);
        }



        public struct SaveUserInfoResponse
        {
            public bool isSuccess;
            public string mesg;
            public int userId;
        }
        /// <summary>
        /// 创建或更新用户信息
        /// </summary>
        /// <param name="openId">需要更新用户信息的用户的唯一标识 OpenID</param>
        /// <param name="avaurl">用户头像链接</param>
        /// <param name="schoolId">学号/工号</param>
        /// <returns></returns>
        public static SaveUserInfoResponse SaveUserInfo(string openId, string avaurl, string schoolId)
        {
            string url = BASE_URL + "user/saveInfo";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["openId"] = openId,
                ["avaurl"] = avaurl,
                ["schoolId"] = schoolId
            }, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<SaveUserInfoResponse>(res);
        }




        public struct GetUserInfoResponse
        {
            public Http.HttpStatus code;
            public string mesg;
            public User user;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static GetUserInfoResponse GetUserInfo(string openId)
        {
            string url = BASE_URL + "user/getInfo";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["openId"] = openId
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetUserInfoResponse>(res);
        }



        public struct GetOpenIdResponse
        {
            public bool isSuccess;
            public string openid;
        }
        /// <summary>
        /// 获取用户openId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static GetOpenIdResponse GetOpenId(string code)
        {
            string url = BASE_URL + "user/getOpenId";
            string res = Http.Get(url, new Dictionary<string, string>
            {
                ["code"] = code
            });
            return JsonConvert.DeserializeObject<GetOpenIdResponse>(res);
        }



        //public struct DeleteUserResponse
        //{

        //}
        ///// <summary>
        ///// 删除用户
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public static DeleteUserResponse DeleteUser(string userId)
        //{
        //    string url = BASE_URL + "user/deleteUser";
        //    string res = Http.Post(url, new Dictionary<string, string>
        //    {
        //        ["userId"] = userId
        //    }, HttpContentType.APPLICATION_X_WWW_FORM_URLENCODED);
        //    return JsonConvert.DeserializeObject<DeleteUserResponse>(res);
        //}



        public struct IsUserExistResponse
        {
            public bool isExist;
        }
        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static IsUserExistResponse IsUserExist(string openId)
        {
            string url = BASE_URL + "user/isExist";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["openId"] = openId
            }, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<IsUserExistResponse>(res);
        }

        #endregion


        #region 主页图片

        public struct GetAdvertisementPhotoResponse
        {
            public bool isSuccess;
            public string[] advertisementPhoto;
        }
        /// <summary>
        /// 获取主页广告图片
        /// </summary>
        /// <returns></returns>
        public static GetAdvertisementPhotoResponse GetAdvertisementPhoto()
        {
            string url = BASE_URL + "advertisement/getAdvertisementPhoto";
            string res = Http.Get(url, null, Headers);
            return JsonConvert.DeserializeObject<GetAdvertisementPhotoResponse>(res);
        }

        #endregion


        #region 项目库

        public struct GetProjectListResponse
        {
            public bool isSuccess;
            public string mesg;
            public Project[] projectList;
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sortRule"></param>
        /// <returns></returns>
        public static GetProjectListResponse GetProjectList(string page, string sortRule)
        {
            string url = BASE_URL + "project/getProjectList";
            string res = Http.Get(url, new Dictionary<string, string>
            {
                ["page"] = page,
                ["sortRule"] = sortRule
            }, Headers);
            return JsonConvert.DeserializeObject<GetProjectListResponse>(res);
        }



        public struct GetSingleProjectInfoResponse
        {
            public bool isSuccess;
            public string mesg;
            public Project project;
        }
        /// <summary>
        /// 获取单个项目的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GetSingleProjectInfoResponse GetSingleProjectInfo(string id)
        {
            string url = BASE_URL + "project/getSingleProjectInfo";
            string res = Http.Get(url, new Dictionary<string, string>
            {
                ["id"] = id
            }, Headers);
            return JsonConvert.DeserializeObject<GetSingleProjectInfoResponse>(res);
        }



        public struct GetUserProjectResponse
        {
            public bool isSuccess;
            public string mesg;
            public Project[] UserProject;
        }
        /// <summary>
        /// 获取用户创建的项目列表
        /// </summary>
        /// <param name="publisherOpenId">用户的openId</param>
        /// <returns></returns>
        public static GetUserProjectResponse GetUserProject(string publisherOpenId)
        {
            string url = BASE_URL + "project/getUserProject";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["publisherOpenId"] = publisherOpenId
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetUserProjectResponse>(res);
        }

        #endregion


        #region 人才库

        public struct GetUserPersonnelResponse
        {
            public bool isSuccess;
            public string mesg;
            public Personnel personnel;
        }
        /// <summary>
        /// 获取用户所创建的人才信息
        /// </summary>
        /// <param name="personnelOpenId">用户的唯一标识 OpenID</param>
        /// <returns></returns>
        public static GetUserPersonnelResponse GetUserPersonnel(string personnelOpenId)
        {
            string url = BASE_URL + "personnel/getUserPersonnel";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["personnelOpenId"] = personnelOpenId
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetUserPersonnelResponse>(res);
        }



        public struct GetPersonnelListResponse
        {
            public bool isSuccess;
            public string mesg;
            public Personnel[] personnelList;
        }
        /// <summary>
        /// 获取人才信息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sortRule"></param>
        /// <returns></returns>
        public static GetPersonnelListResponse GetPersonnelList(string page, string sortRule)
        {
            string url = BASE_URL + "personnel/getPersonnelList";
            string res = Http.Get(url, new Dictionary<string, string>
            {
                ["page"] = page,
                ["sortRule"] = sortRule
            }, Headers);
            return JsonConvert.DeserializeObject<GetPersonnelListResponse>(res);
        }



        public struct GetPersonnelTechnologyTagsResponse
        {
            public bool isSuccess;
            public TechnologyTagItem[] technologyTags;
        }
        /// <summary>
        /// 获取人才技能标签数组
        /// </summary>
        /// <returns></returns>
        public static GetPersonnelTechnologyTagsResponse GetPersonnelTechnologyTags()
        {
            string url = BASE_URL + "personnel/getPersonnelTechnologyTags";
            string res = Http.Get(url);
            return JsonConvert.DeserializeObject<GetPersonnelTechnologyTagsResponse>(res);
        }

        #endregion


        #region 后台管理

        public struct AdministratorLoginResponse
        {
            public Http.HttpStatus code;
            public string token;
            public string mesg;
        }
        /// <summary>
        /// 后台管理员登录
        /// </summary>
        /// <param name="administratorId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static AdministratorLoginResponse AdministratorLogin(string administratorId, string password)
        {
            string url = BASE_URL + "administrator/administratorLogin";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["administratorId"] = administratorId,
                ["password"] = password
            }, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AdministratorLoginResponse>(res);
        }



        public struct GetVerificationCodeResponse
        {
            public string code;
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 人工验证获取验证码
        /// </summary>
        /// <param name="schoolId">学号/工号</param>
        /// <returns></returns>
        public static GetVerificationCodeResponse GetVerificationCode(string schoolId)
        {
            string url = BASE_URL + "administrator/getCode";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["schoolId"] = schoolId
            }, AdminApiHeaders, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetVerificationCodeResponse>(res);
        }

        #endregion


    }
}
