namespace PDT_WPF.Models
{
    /// <summary>
    /// 项目主要技术
    /// </summary>
    public class ProjectKeyword
    {
        public string KeywordId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string Keyword { get; set; }
        public object Projects { get; set; }
    }
}
