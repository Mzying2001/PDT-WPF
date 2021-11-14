namespace PDT_WPF.Models
{
    /// <summary>
    /// 首页轮播图信息
    /// </summary>
    public class BoardPhoto
    {
        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Link { get; set; }
        public JumpType Jump { get; set; }



        public enum JumpType
        {
            /// <summary>
            /// 转跳链接
            /// </summary>
            JumpLink = 1,

            /// <summary>
            /// 转跳其他小程序
            /// </summary>
            JumpMiniProgram = 2,

            /// <summary>
            /// 不转跳
            /// </summary>
            NoJump = 3,
        }

        public static string GetJumpTypeName(JumpType jumpType)
        {
            switch (jumpType)
            {
                case JumpType.NoJump:
                    return "不跳转";

                case JumpType.JumpLink:
                    return "公众号推文";

                case JumpType.JumpMiniProgram:
                    return "小程序";

                default:
                    throw new System.Exception("错误的JumpType。");
            }
        }
    }
}
