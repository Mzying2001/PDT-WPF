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
        public string SchoolId { get; set; }
        public int Power { get; set; }
        public string AdministratorId { get; set; }
        public string Password { get; set; }
        public string LastActive { get; set; }
        public string Description { get; set; }
        public object MyLikeProjects { get; set; }
        public object MyLikePersonnels { get; set; }
        public object MyCollectProjects { get; set; }
        public object MyCollectPersonnels { get; set; }
        public object MyOffer { get; set; }
        public object MyOffer2 { get; set; }
        public object MyForum { get; set; }
        public object MyLikeForums { get; set; }
        public object MyFollowForums { get; set; }
        public object MyLikeForumComments { get; set; }
        public object MyLikeTalkTags { get; set; }
        public object MyFollowTalkTags { get; set; }
    }
}
