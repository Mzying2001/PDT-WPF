namespace PDT_WPF.Models
{
    /// <summary>
    /// 论坛话题标签
    /// </summary>
    public class TalkTagItem
    {
        public int TalkTagId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public int UserId { get; set; }
        public int Code { get; set; }
        public string TalkTag { get; set; }
        public string Apply { get; set; }
        public int Views { get; set; }
        public int ForumNum { get; set; }
        public int LikeNum { get; set; }
        public int FollowNum { get; set; }
        public object WhoFollow { get; set; }
        public object Forums { get; set; }
    }
}
