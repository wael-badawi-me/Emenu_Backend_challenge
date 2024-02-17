
using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase
{
    public class DataContext : DbContext
    {
        #region Attributes
        private readonly IConfiguration config;
        private string key { get; set; }
        #endregion

        public DataContext()
        {

        }

        public DataContext(IConfiguration _config)
        {
            config = _config;
            key = config.GetSection("ConnectionStrings").GetSection("Key").Value.ToString();

        }



        #region DbSets
        public DbSet<ColorEntity> colors { get; set; }
        public DbSet<MaterialEntity> materials { get; set; }
        public DbSet<PhotoEntity> photos { get; set; }
        public DbSet<ProductEntity> products { get; set; }
        public DbSet<ProductPhotoEntity> productPhotos { get; set; }
        public DbSet<SizeEntity> sizes { get; set; }
        public DbSet<StoreEntity> stores { get; set; }
        public DbSet<StorePhotoEntity> storePhotos { get; set; }
        #endregion


        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder Builder)
        {
            base.OnConfiguring(Builder);

            Builder.UseSqlServer(key);

        }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            base.OnModelCreating(Builder);

            Builder.Entity<StorePhotoEntity>().HasOne<StoreEntity>(m => m.Store).WithMany().HasForeignKey(m => m.StoreId).OnDelete(DeleteBehavior.NoAction);
            
            Builder.Entity<ProductPhotoEntity>().HasOne<ProductEntity>(m => m.Product).WithMany().HasForeignKey(m => m.ProductId).OnDelete(DeleteBehavior.NoAction);




        }


        #endregion


    }
}
