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
        }
        /// <summary>
        /// 添加首页轮播图
        /// </summary>
        /// <param name="name">轮播图名称</param>
        /// <param name="photo">轮播图</param>
        /// <param name="link">详情页链接</param>
        /// <returns></returns>
        public static AddBoardPhotoResponse AddBoardPhoto(string name, string photo, string link)
        {
            throw new System.Exception("未完成");
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
            string res = Http.Delete(url);
            return JsonConvert.DeserializeObject<DeleteBoardPhotoResponse>(res);
        }

        #endregion


    }
}
