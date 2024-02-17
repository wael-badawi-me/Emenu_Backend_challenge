using Database.Seed.IService;
using DataBase;
using DataBase.Entities;
using Shared.Enums;


namespace Database.Seed.Service
{
    public class SeedService : ISeedService
    {
        private readonly DataContext Context;

        public SeedService(DataContext _Context)
        {
            Context = _Context;
        }

        public async Task CreateColorsAsync()
        {
            var Colors = Enum.GetNames(typeof(Color));
            var ColorsSet = new List<ColorEntity>();
            foreach (var color in Colors)
            {
                ColorsSet.Add(new ColorEntity()
                {
                    Name=color
                });
            }
            await Context.colors.AddRangeAsync(ColorsSet);
            await Context.SaveChangesAsync();
        }

        public async Task CreateMaterialsAsync()
        {
            var Materials = Enum.GetNames(typeof(Material));
            var MaterialsSet = new List<MaterialEntity>();
            foreach (var Material in Materials)
            {
                MaterialsSet.Add(new MaterialEntity()
                {
                    Name = Material
                });
            }
            await Context.materials.AddRangeAsync(MaterialsSet);
            await Context.SaveChangesAsync();
        }

        public async Task CreateSizesAsync()
        {
            var Sizes = Enum.GetNames(typeof(Size));
            var SizesSet = new List<SizeEntity>();
            foreach (var size in Sizes)
            {
                SizesSet.Add(new SizeEntity()
                {
                    Name = size
                });
            }
            await Context.sizes.AddRangeAsync(SizesSet);
            await Context.SaveChangesAsync();
        }
    }
}
