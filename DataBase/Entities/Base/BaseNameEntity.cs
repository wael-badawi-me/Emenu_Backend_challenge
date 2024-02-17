

namespace DataBase.Entities.Base
{
    public class BaseNameEntity : BaseEntity
    {
        [Required]
        public string NameEn { get; set; }
        [Required]
        public string NameAr { get; set; }    

    }
}
