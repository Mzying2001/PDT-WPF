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
    }
}
