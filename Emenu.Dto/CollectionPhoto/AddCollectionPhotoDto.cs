namespace Emenu.Dto.CollectionPhoto
{
    public class AddCollectionPhotoDto
    {
        public int id { get; set; }
        public int collectionId { get; set; }
        public List<string> photos { get; set; }
    }
}
