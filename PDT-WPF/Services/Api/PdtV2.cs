using Newtonsoft.Json;
using PDT_WPF.Models;
using System.Collections.Generic;
using static PDT_WPF.Services.Api.PdtCommon;

namespace PDT_WPF.Services.Api
{
    public static class PdtV2
    {
        public const string BASE_URL = "https://pdt.ojbk.me/api/v2/";

        #region 首页

        /// <summary>
        /// 获取首页轮播图
        /// </summary>
        /// <returns></returns>
        public static BoardPhoto[] GetBoardPhotos()
        {
            string url = BASE_URL + "homePage/boardPhoto";
            string res = Http.Get(url, null, Headers);
            return JsonConvert.DeserializeObject<BoardPhoto[]>(res);
        }



        /// <summary>
        /// 获取比赛栏信息
        /// </summary>
        /// <returns></returns>
        public static CompetitionSection[][] GetCompetitionSections()
        {
            string url = BASE_URL + "homePage/competitionSection";
            string res = Http.Get(url, null, Headers);
            return JsonConvert.DeserializeObject<CompetitionSection[][]>(res);
        }

        #endregion


        #region 后台管理

        public struct AddBoardPhotoResponse
        {
            public Http.HttpStatus code;
            public string mesg;
            public AddBoardPhotoErrors errors;
        }
        public struct AddBoardPhotoErrors
        {
            public string Link;
            public string Jump;
        }
        /// <summary>
        /// 添加首页轮播图
        /// </summary>
        /// <param name="name">轮播图名称</param>
        /// <param name="link">详情页链接</param>
        /// <param name="filePath">轮播图路径（本地）</param>
        /// <param name="jump">转跳类型参数</param>
        /// <returns></returns>
        public static AddBoardPhotoResponse AddBoardPhoto(string name, string link, string filePath, BoardPhoto.JumpType jump)
        {
            string url = BASE_URL + "homePage/boardPhoto";
            string res = Http.UploadFile(url, "photo", filePath, new Dictionary<string, string>
            {
                ["name"] = name,
                ["link"] = link,
                ["jump"] = ((int)jump).ToString()
            }, AdminApiHeaders);
            return JsonConvert.DeserializeObject<AddBoardPhotoResponse>(res);
        }



        public struct DeleteBoardPhotoResponse
        {
            public Http.HttpStatus code;
            public string mesg;    //成功信息
            public string message; //错误信息
        }
        /// <summary>
        /// 删除首页轮播图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DeleteBoardPhotoResponse DeleteBoardPhoto(int id)
        {
            string url = $"{BASE_URL}homePage/boardPhoto/{id}";
            string res = Http.Delete(url, null, AdminApiHeaders);
            return JsonConvert.DeserializeObject<DeleteBoardPhotoResponse>(res);
        }



        public struct AddCompetitionSectionResponse
        {
            public Http.HttpStatus code;
            public string mesg;
            public AddCompetitionSectionErrors errors;
        }
        public struct AddCompetitionSectionErrors
        {
            public string Link;
        }
        /// <summary>
        /// 添加比赛栏信息
        /// </summary>
        /// <param name="title">信息标题</param>
        /// <param name="information">信息正文</param>
        /// <returns></returns>
        public static AddCompetitionSectionResponse AddCompetitionSection(string title, string information)
        {
            string url = BASE_URL + "homePage/competitionSection";
            string res = Http.Post(url, new Dictionary<string, string>
            {
                ["title"] = title,
                ["information"] = information
            }, AdminApiHeaders, Http.ContentType.APPLICATION_X_WWW_FORM_URLENCODED);
            return JsonConvert.DeserializeObject<AddCompetitionSectionResponse>(res);
        }



        public struct DeleteCompetitionSectionResponse
        {
            public Http.HttpStatus code;
            public string mesg;    //成功信息
            public string message; //失败信息
        }
        /// <summary>
        /// 删除比赛栏信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DeleteCompetitionSectionResponse DeleteCompetitionSection(int id)
        {
            string url = $"{BASE_URL}homePage/competitionSection/{id}";
            string res = Http.Delete(url, null, AdminApiHeaders);
            return JsonConvert.DeserializeObject<DeleteCompetitionSectionResponse>(res);
        }

        #endregion


    }
}
