namespace PDT_WPF.Models
{
    /// <summary>
    /// 论坛举报信息
    /// </summary>
    public class ForumReport
    {
        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public int ForumId { get; set; }
        public ForumPost Forum { get; set; }
        public int WhoTipId { get; set; }
        public User WhoTip { get; set; }
    }
}
