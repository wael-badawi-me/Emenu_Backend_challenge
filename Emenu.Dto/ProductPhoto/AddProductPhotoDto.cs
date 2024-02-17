namespace Emenu.Dto.ProductPhoto
{
    public class AddProductPhotoDto
    {
        public int id { get; set; }
        public int productId { get; set; }
        public List<string> photos { get; set; }
    }
}
