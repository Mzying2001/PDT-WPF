using Newtonsoft.Json;
using PDT_WPF.Models;

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



        #endregion


    }
}
