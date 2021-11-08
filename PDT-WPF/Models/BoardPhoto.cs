namespace PDT_WPF.Models
{
    /// <summary>
    /// 首页轮播图信息
    /// </summary>
    public class BoardPhoto
    {
        public enum JumpType
        {
            /// <summary>
            /// 不转跳
            /// </summary>
            NoJump = 0,

            /// <summary>
            /// 转跳链接
            /// </summary>
            JumpLink = 1,

            /// <summary>
            /// 转跳其他小程序
            /// </summary>
            JumpMiniProgram = 2
        }



        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Link { get; set; }
        public JumpType Jump { get; set; }
    }
}
