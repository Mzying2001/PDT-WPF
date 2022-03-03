namespace PDT_WPF.Models
{
    /// <summary>
    /// 项目赛事
    /// </summary>
    public class ProjectMatch
    {
        public int MatchId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string MatchName { get; set; }
        public object Projects { get; set; }
    }
}
