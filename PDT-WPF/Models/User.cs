namespace PDT_WPF.Models
{
    public class User
    {
        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
        public string Gender { get; set; }
        public string College { get; set; }
        public string Major { get; set; }
        public string EMail { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Language { get; set; }
        public string SelfIntroduce { get; set; }
        public string Avaurl { get; set; }
        public object MyLikeProjects { get; set; }
        public object MyLikePersonnels { get; set; }
        public object MyCollectProjects { get; set; }
        public object MyCollectPersonnels { get; set; }
    }
}
