
namespace DataBase.Entities
{
    public class ProductEntity:BaseNameEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        [ForeignKey("MainPhotos")]
        public int MainPhotoId { get; set; }
        public virtual PhotoEntity MainPhoto { get; set; }

    }
}
