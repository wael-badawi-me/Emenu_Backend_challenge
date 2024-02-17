namespace Database.Seed.IService
{
    public interface ISeedService
    {
        Task CreateColorsAsync();
        Task CreateMaterialsAsync();
        Task CreateSizesAsync();
    }
}
