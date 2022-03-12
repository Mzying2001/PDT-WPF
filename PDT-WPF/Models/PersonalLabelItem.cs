namespace PDT_WPF.Models
{
    /// <summary>
    /// 人才个人标签
    /// </summary>
    public class PersonalLabelItem
    {
        public int ID { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string PersonalLabel { get; set; }
        public object Personnels { get; set; }
    }
}
