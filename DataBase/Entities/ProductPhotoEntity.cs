
namespace DataBase.Entities
{
    public class ProductPhotoEntity:BaseEntity
    {
        //[Required]
        //[ForeignKey("Products")]
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }


        [Required]
        [ForeignKey("Photos")]
        public int PhotoId { get; set; }
        public PhotoEntity Photo { get; set; }


    }
}
