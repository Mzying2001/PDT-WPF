namespace PDT_WPF.Models
{
    /// <summary>
    /// 帖子的自定义标签
    /// </summary>
    public class CustomTagItem
    {
        public int CustomTagId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public int ForumId { get; set; }
        public string CustomTag { get; set; }
    }
}
