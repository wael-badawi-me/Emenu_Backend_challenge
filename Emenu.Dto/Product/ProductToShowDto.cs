using Emenu.Dto.Photo;

namespace Emenu.Dto.Product
{
    public class ProductToShowDto
    {
        public int id { get; set; }
        public string nameAr { get; set; }
        public string nameEn { get; set; }
        public string description { get; set; }
        public string mainPhotoUrl { get; set; }
        public List<PhotoDto> photoes { get; set; }
    }
}
