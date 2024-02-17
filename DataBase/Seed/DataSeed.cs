using Database.Seed.IService;
using Database.Seed.Service;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Seed
{
    public static class DataSeed
    {

        private static void ConfigureServices(string connectionString, IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString,
               sqlDbOptions => sqlDbOptions.MigrationsAssembly("Emenu.SqlServer")));

            services.AddScoped<ISeedService, SeedService>();
        }

        public async static Task EnsureDatabaseExistAsync(DataContext context)
        {
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await context.Database.MigrateAsync();
            }
        }

        public async static Task<bool> SeedDataAsync(string connectionString, IServiceCollection services)
        {
            ConfigureServices(connectionString, services);

            var provider = services.BuildServiceProvider();

            var context = provider.GetService<DataContext>();

            await EnsureDatabaseExistAsync(context);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!context.colors.Any())
                    {
                        await SeedColorAsync(provider);
                    }
                    if (!context.materials.Any())
                    {
                        await SeedMaterialAsync(provider);
                    }
                    if (!context.sizes.Any())
                    {
                        await SeedSizeAsync(provider);
                    }

                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();

                    return false;
                }
            }
        }


        private async static Task SeedColorAsync(ServiceProvider serviceProvider)
        {
            var Service = serviceProvider.GetService<ISeedService>();
            await Service.CreateColorsAsync();
        }
        private async static Task SeedMaterialAsync(ServiceProvider serviceProvider)
        {
            var Service = serviceProvider.GetService<ISeedService>();
            await Service.CreateMaterialsAsync();
        }
        private async static Task SeedSizeAsync(ServiceProvider serviceProvider)
        {
            var Service = serviceProvider.GetService<ISeedService>();
            await Service.CreateSizesAsync();
        }
    }
}
