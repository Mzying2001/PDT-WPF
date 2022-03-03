namespace PDT_WPF.Models
{
    /// <summary>
    /// 项目主要技术
    /// </summary>
    public class ProjectMainTechnology
    {
        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string MainTechnology { get; set; }
        public object Projects { get; set; }
    }
}
