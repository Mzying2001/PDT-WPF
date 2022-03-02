namespace PDT_WPF.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public int OwnerID { get; set; }
        public User Owner { get; set; }
        public string ProjectName { get; set; }
        public string ResponsibilityPersonName { get; set; }
        public string BriefIntroduction { get; set; }
        public string AwardRecord { get; set; }
        public string Requirement { get; set; }
        public string Contact { get; set; }
        public string College { get; set; }
        public string Threshold { get; set; }
        public string MainTechnology { get; set; }
        public string ProjectType { get; set; }
        public string BelongTo { get; set; }
        public string ProjectState { get; set; }
        public string PhotoPath { get; set; }
        public int LikeNum { get; set; }
        public int CollectNum { get; set; }
        public ProjectKeyword[] Keywords { get; set; }
        public object[] ProjectAttachments { get; set; }
        public object UsersWhoCollectIt { get; set; }
        public object Comments { get; set; }
    }
}
