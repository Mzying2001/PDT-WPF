namespace PDT_WPF.Models
{
    /// <summary>
    /// 帖子中的图片
    /// </summary>
    public class ForumPhoto
    {
        public int ForumPhotoId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public int ForumId { get; set; }
        public string PhotoPath { get; set; }
    }
}
