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



        public struct GetProjectMatchResponse
        {
            public Http.HttpStatus code;
            public ProjectMatch[] projectMatch;
            public string mesg;
        }
        /// <summary>
        /// 获取项目赛事数组
        /// </summary>
        /// <returns></returns>
        public static GetProjectMatchResponse GetProjectMatch()
        {
            string url = BASE_URL + "project/ProjectMatch";
            string res = Http.Get(url, null, Headers);
            return JsonConvert.DeserializeObject<GetProjectMatchResponse>(res);
        }



        public struct GetProjectMainTechnologyResponse
        {
            public Http.HttpStatus code;
            public ProjectMainTechnology[] mainTechnologys;
            public string mesg;
        }
        /// <summary>
        /// 获取项目主要技术数组
        /// </summary>
        /// <returns></returns>
        public static GetProjectMainTechnologyResponse GetProjectMainTechnology()
        {
            string url = BASE_URL + "project/ProjectMainTechnology";
            string res = Http.Get(url, null, Headers);
            return JsonConvert.DeserializeObject<GetProjectMainTechnologyResponse>(res);
        }



        public struct GetProjectTypeResponse
        {
            public Http.HttpStatus code;
            public ProjectTypeItem[] projectTypes;
            public string mesg;
        }
        /// <summary>
        /// 获取项目类型标签
        /// </summary>
        /// <returns></returns>
        public static GetProjectTypeResponse GetProjectType()
        {
            string url = BASE_URL + "project/ProjectType";
            string res = Http.Get(url, null, Headers, Http.ContentType.MULTIPART_FORM_DATA);
            return JsonConvert.DeserializeObject<GetProjectTypeResponse>(res);
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
            public Http.HttpStatus code;
            public TechnologyTagItem[] mainTechnologys;
            public string mesg;
        }
        /// <summary>
        /// 获取人才技能标签数组
        /// </summary>
        /// <returns></returns>
        public static GetPersonnelTechnologyTagsResponse GetPersonnelTechnologyTags()
        {
            string url = BASE_URL + "personnel/PersonnelTechnologyTag";
            string res = Http.Get(url, null, Headers);
            return JsonConvert.DeserializeObject<GetPersonnelTechnologyTagsResponse>(res);
        }



        public struct GetPersonalLabelResponse
        {
            public Http.HttpStatus code;
            public PersonalLabelItem[] personalLabel;
            public string mesg;
        }
        /// <summary>
        /// 获取人才个人标签
        /// </summary>
        /// <returns></returns>
        public static GetPersonalLabelResponse GetPersonalLabel()
        {
            string url = BASE_URL + "personnel/personalLabel";
            string res = Http.Get(url, null, Headers, Http.ContentType.MULTIPART_FORM_DATA);
            return JsonConvert.DeserializeObject<GetPersonalLabelResponse>(res);
        }

        #endregion


        #region 论坛

        public struct GetForumTalkTagResponse
        {
            public bool isSuccess;
            public TalkTagItem[] talkTags;
            public string mesg;
        }
        /// <summary>
        /// 获取全部的话题标签
        /// </summary>
        /// <returns></returns>
        public static GetForumTalkTagResponse GetForumTalkTag()
        {
            string url = BASE_URL + "forum/getForumTalkTaga";
            string res = Http.Get(url, null, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetForumTalkTagResponse>(res);
        }



        public struct GetForumResponse
        {
            public ForumPost[] forums;
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 获取论坛列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="sortRule">排序方式 0:时间降序,1:热度降序,2:时间升序,3:热度升序</param>
        /// <returns></returns>
        public static GetForumResponse GetForum(int page, int sortRule)
        {
            string url = BASE_URL + "forum/getForum";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["sortRule"] = sortRule.ToString()
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetForumResponse>(res);
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
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetVerificationCodeResponse>(res);
        }



        public struct AddProjectMainTechnologyResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 添加项目主要技术标签
        /// </summary>
        /// <param name="mainTechnology">项目主要技术</param>
        /// <returns></returns>
        public static AddProjectMainTechnologyResponse AddProjectMainTechnology(string mainTechnology)
        {
            string url = BASE_URL + "project/ProjectMainTechnology";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["mainTechnology"] = mainTechnology
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AddProjectMainTechnologyResponse>(res);
        }



        public struct DeleteProjectMainTechnologyResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 删除项目主要技术标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DeleteProjectMainTechnologyResponse DeleteProjectMainTechnology(int id)
        {
            string url = BASE_URL + $"project/ProjectMainTechnology/{id}";
            string res = Http.Delete(url, null, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<DeleteProjectMainTechnologyResponse>(res);
        }



        public struct ChangeProjectMainTechnologyResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 修改项目主要技术标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mainTechnology">项目主要技术</param>
        /// <returns></returns>
        public static ChangeProjectMainTechnologyResponse ChangeProjectMainTechnology(int id, string mainTechnology)
        {
            string url = BASE_URL + $"project/ProjectMainTechnology/{id}";
            string res = Http.Put(url, new Dictionary<string, string>
            {
                ["mainTechnology"] = mainTechnology
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<ChangeProjectMainTechnologyResponse>(res);
        }



        public struct AddProjectTypeResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 添加项目类型标签
        /// </summary>
        /// <param name="projectType">项目类型</param>
        /// <returns></returns>
        public static AddProjectTypeResponse AddProjectType(string projectType)
        {
            string url = BASE_URL + "project/ProjectType";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["projectType"] = projectType
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AddProjectTypeResponse>(res);
        }



        public struct DeleteProjectTypeResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 删除项目类型标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DeleteProjectTypeResponse DeleteProjectType(int id)
        {
            string url = BASE_URL + $"project/ProjectType/{id}";
            string res = Http.Delete(url, null, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<DeleteProjectTypeResponse>(res);
        }



        public struct ChangeProjectTypeResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 修改项目类型标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectType">项目类型</param>
        /// <returns></returns>
        public static ChangeProjectTypeResponse ChangeProjectType(int id, string projectType)
        {
            string url = BASE_URL + $"/project/ProjectType/{id}";
            string res = Http.Put(url, new Dictionary<string, string>
            {
                ["projectType"] = projectType
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<ChangeProjectTypeResponse>(res);
        }



        public struct AddProjectMatchResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 添加项目赛事标签
        /// </summary>
        /// <param name="matchName">项目赛事</param>
        /// <returns></returns>
        public static AddProjectMatchResponse AddProjectMatch(string matchName)
        {
            string url = BASE_URL + "project/ProejctMatch";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["matchName"] = matchName
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AddProjectMatchResponse>(res);
        }



        public struct DeleteProjectMatchResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 删除项目赛事标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DeleteProjectMatchResponse DeleteProjectMatch(int id)
        {
            string url = BASE_URL + $"project/ProejctMatch/{id}";
            string res = Http.Delete(url, null, Headers);
            return JsonConvert.DeserializeObject<DeleteProjectMatchResponse>(res);
        }



        public struct ChangeProjectMatchResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 修改项目赛事标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="matchName">项目赛事标签</param>
        /// <returns></returns>
        public static ChangeProjectMatchResponse ChangeProjectMatch(int id, string matchName)
        {
            string url = BASE_URL + $"project/ProejctMatch/{id}";
            string res = Http.Put(url, new Dictionary<string, string>
            {
                ["matchName"] = matchName
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<ChangeProjectMatchResponse>(res);
        }



        public struct AddPersonnelTechnologyTagResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 添加人才技能标签
        /// </summary>
        /// <param name="technologyTag">人才技能标签</param>
        /// <returns></returns>
        public static AddPersonnelTechnologyTagResponse AddPersonnelTechnologyTag(string technologyTag)
        {
            string url = BASE_URL + "personnel/PersonnelTechnologyTag";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["technologyTag"] = technologyTag
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AddPersonnelTechnologyTagResponse>(res);
        }



        public struct DeletePersonnelTechnologyTagResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 删除人才技能标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DeletePersonnelTechnologyTagResponse DeletePersonnelTechnologyTag(int id)
        {
            string url = BASE_URL + $"personnel/PersonnelTechnologyTag/{id}";
            string res = Http.Delete(url, null, Headers);
            return JsonConvert.DeserializeObject<DeletePersonnelTechnologyTagResponse>(res);
        }



        public struct ChangePersonnelTechnologyTagResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 修改人才技能标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="technologyTag">人才技能标签</param>
        /// <returns></returns>
        public static ChangePersonnelTechnologyTagResponse ChangePersonnelTechnologyTag(int id, string technologyTag)
        {
            string url = BASE_URL + $"personnel/PersonnelTechnologyTag/{id}";
            string res = Http.Put(url, new Dictionary<string, string>
            {
                ["technologyTag"] = technologyTag
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<ChangePersonnelTechnologyTagResponse>(res);
        }



        public struct AddAdministratorResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 添加后台管理员账户
        /// </summary>
        /// <param name="administratorId">管理员账户名</param>
        /// <param name="password">管理员账户密码</param>
        /// <param name="description">管理员账号描述</param>
        /// <returns></returns>
        public static AddAdministratorResponse AddAdministrator(string administratorId, string password, string description)
        {
            string url = BASE_URL + "administrator/addAdministrator";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["administratorId"] = administratorId,
                ["password"] = password,
                ["description"] = description
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AddAdministratorResponse>(res);
        }



        public struct GetForumTalkTagApplyListResponse
        {
            public bool isSuccess;
            public string mesg;
            public TalkTagItem[] talkTags;
        }
        /// <summary>
        /// 获取未同意的话题标签
        /// </summary>
        /// <returns></returns>
        public static GetForumTalkTagApplyListResponse GetForumTalkTagApplyList()
        {
            string url = BASE_URL + "forum/getForumTalkTagb";
            string res = Http.Get(url, null, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetForumTalkTagApplyListResponse>(res);
        }



        public struct ProcessTalkTagAppyResponse
        {
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 同意话题申请
        /// </summary>
        /// <param name="talkTagId">话题标签id</param>
        /// <param name="agree">是否同意</param>
        /// <returns></returns>
        public static ProcessTalkTagAppyResponse ProcessTalkTagAppy(int talkTagId, bool agree)
        {
            string url = BASE_URL + "forum/agreeTalkTag";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["talkTagId"] = talkTagId.ToString(),
                ["code"] = agree ? "1" : "0"
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<ProcessTalkTagAppyResponse>(res);
        }



        public struct AddForumTalkTagResponse
        {
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 增加话题标签
        /// </summary>
        /// <param name="talkTag">话题标签</param>
        /// <param name="apply">一句话介绍</param>
        /// <returns></returns>
        public static AddForumTalkTagResponse AddForumTalkTag(string talkTag, string apply)
        {
            string url = BASE_URL + "forum/addForumTalkTagb";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["talkTag"] = talkTag,
                ["apply"] = apply
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AddForumTalkTagResponse>(res);
        }



        public struct DeleteForumTalkTagResponse
        {
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 删除话题标签
        /// </summary>
        /// <param name="talkTagId">话题标签id</param>
        /// <returns></returns>
        public static DeleteForumTalkTagResponse DeleteForumTalkTag(int talkTagId)
        {
            string url = BASE_URL + "forum/deleteForumTalkTagb";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["talkTagId"] = talkTagId.ToString()
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<DeleteForumTalkTagResponse>(res);
        }



        public struct GetUnapprovedForumPostsResponse
        {
            public ForumPost[] forums;
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 获取未操作的论坛列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public static GetUnapprovedForumPostsResponse GetUnapprovedForumPosts(int page)
        {
            var url = BASE_URL + "forum/getUnForum";
            var res = Http.Post(url, new Dictionary<string, string>
            {
                ["page"] = page.ToString()
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetUnapprovedForumPostsResponse>(res);
        }



        public struct ProcessUnapprovedForumPostsResponse
        {
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 同意发布帖子
        /// </summary>
        /// <param name="forumId">帖子id</param>
        /// <param name="agree">是否同意</param>
        /// <returns></returns>
        public static ProcessUnapprovedForumPostsResponse ProcessUnapprovedForumPosts(int forumId, bool agree)
        {
            string url = BASE_URL + "forum/agreeForum";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["forumId"] = forumId.ToString(),
                ["code"] = agree ? "1" : "0"
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<ProcessUnapprovedForumPostsResponse>(res);
        }



        public struct AddPersonalLabelResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 添加人才个人标签
        /// </summary>
        /// <param name="personalLabel">人才个人标签</param>
        /// <returns></returns>
        public static AddPersonalLabelResponse AddPersonalLabel(string personalLabel)
        {
            string url = BASE_URL + "personnel/personalLabel";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["personalLabel"] = personalLabel
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AddPersonalLabelResponse>(res);
        }



        public struct DeletePersonalLabelResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 删除人才个人标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DeletePersonalLabelResponse DeletePersonalLabel(int id)
        {
            string url = BASE_URL + $"personnel/personalLabel/{id}";
            string res = Http.Delete(url, null, Headers, Http.ContentType.MULTIPART_FORM_DATA);
            return JsonConvert.DeserializeObject<DeletePersonalLabelResponse>(res);
        }



        public struct ChangePersonalLabelResponse
        {
            public Http.HttpStatus code;
            public string mesg;
        }
        /// <summary>
        /// 修改人才个人标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="personalLabel">人才个人标签</param>
        /// <returns></returns>
        public static ChangePersonalLabelResponse ChangePersonalLabel(int id, string personalLabel)
        {
            string url = BASE_URL + $"personnel/personalLabel/{id}";
            string res = Http.Put(url, new Dictionary<string, string>
            {
                ["personalLabel"] = personalLabel
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<ChangePersonalLabelResponse>(res);
        }



        public struct GetTipForumResponse
        {
            public bool isSuccess;
            public string mesg;
            public ForumPost[] tipforums;
        }
        /// <summary>
        /// 获取被举报的帖子列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public static GetTipForumResponse GetTipForum(int page)
        {
            string url = BASE_URL + "forum/getTipForum";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["page"] = page.ToString()
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<GetTipForumResponse>(res);
        }



        public struct IgnoreForumResponse
        {
            public bool isSuccess;
            public string mesg;
        }
        /// <summary>
        /// 忽略被举报的帖子
        /// </summary>
        /// <param name="id">帖子id</param>
        /// <returns></returns>
        public static IgnoreForumResponse IgnoreForum(int id)
        {
            string url = BASE_URL + "forum/ignoreForum";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["Id"] = id.ToString()
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<IgnoreForumResponse>(res);
        }



        public struct DeleteTipForumResponse
        {
            public bool isSuccess;
            public string mesg;
        };
        /// <summary>
        /// 删除帖子(同时忽略该帖所有举报)
        /// </summary>
        /// <param name="id">帖子id</param>
        /// <returns></returns>
        public static DeleteTipForumResponse DeleteTipForum(int id)
        {
            string url = BASE_URL + "forum/deleteTipForum";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["Id"] = id.ToString()
            }, Headers, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<DeleteTipForumResponse>(res);
        }

        #endregion


    }
}
