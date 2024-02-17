
namespace DataBase.Entities
{
    public class StorePhotoEntity:BaseEntity
    {
        //[Required]
        //[ForeignKey("Stores")]
        public int StoreId { get; set; }


        public virtual StoreEntity Store { get; set; }

        [Required]
        [ForeignKey("Photos")]
        public int PhotoId { get; set; }
        public virtual PhotoEntity Photo { get; set; }

    }
}
