
namespace DataBase.Entities
{
    public class StoreEntity:BaseEntity
    {
        [Required]
        [ForeignKey("Products")]
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }

        [Required]
        [ForeignKey("Sizes")]
        public int SizeId { get; set; }
        public virtual SizeEntity Size { get; set; }

        [Required]
        [ForeignKey("Colors")]
        public int ColorId { get; set; }
        public virtual ColorEntity Color { get; set; }

        [Required]
        [ForeignKey("Materials")]
        public int MaterialId { get; set; }
        public virtual MaterialEntity Material { get; set; }
    }
}
