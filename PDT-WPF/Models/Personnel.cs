namespace PDT_WPF.Models
{
    public class Personnel
    {
        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public int OwnerID { get; set; }
        public User Owner { get; set; }
        public string TrueName { get; set; }
        public string Gender { get; set; }
        public string College { get; set; }
        public string Major { get; set; }
        public string Grade { get; set; }
        public string BriefIntroduction { get; set; }
        public string SelfIntroduce { get; set; }
        public string PhotoPath { get; set; }
        public string Intention { get; set; }
        public string Contact { get; set; }
        public string ProjectExperience { get; set; }
        public string AwardRecord { get; set; }
        public string Hobby { get; set; }
        public int LikeNum { get; set; }
        public int CollectNum { get; set; }
        public TechnologyTagItem[] TechnologyTags { get; set; }
        public object[] PersonnelAttachments { get; set; }
        public object UsersWhoCollectIt { get; set; }
    }
}
