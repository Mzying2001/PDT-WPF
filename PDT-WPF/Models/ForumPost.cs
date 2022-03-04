namespace PDT_WPF.Models
{
    /// <summary>
    /// 论坛帖子
    /// </summary>
    public class ForumPost
    {
        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public int OwnerID { get; set; }
        public User Owner { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public CustomTagItem[] CustomTags { get; set; }
        public TalkTagItem[] TalkTags { get; set; }
        public ForumPhoto[] ForumPhotos { get; set; }
        public object ForumComments { get; set; }
        public object WhoFollow { get; set; }
        public int CommentNum { get; set; }
        public int LikeNum { get; set; }
        public int FollowNum { get; set; }
        public int Heat { get; set; }
    }
}
