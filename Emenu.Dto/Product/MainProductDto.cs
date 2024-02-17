using Emenu.Dto.Photo;

namespace Emenu.Dto.Product
{
    public class MainProductDto
    {
        public int id { get; set; }
        public string nameAr { get; set; }
        public string nameEn { get; set; }
        public string description { get; set; }
        public string mainPhotoUrl { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public string material { get; set; }
        public List<PhotoDto> photoes { get; set; }

    }
}
