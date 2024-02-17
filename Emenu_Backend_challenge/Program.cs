using Database.Seed;
using DataBase;
using Emenu.IRepo.IData;
using Emenu.Repo.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Key");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<DataBase.DataContext>();

builder.Services.AddTransient<IColor, ColorRepo>();
builder.Services.AddTransient<ISize, SizeRepo>();
builder.Services.AddTransient<IMaterial, MaterialRepo>();
builder.Services.AddTransient<IProduct, ProductRepo>();
builder.Services.AddTransient<IPhoto, PhotoRepo>();
builder.Services.AddTransient<IProductPhoto, ProductPhotoRepo>();
builder.Services.AddTransient<ICollectionPhoto, StorePhotoRepo>();
builder.Services.AddTransient<ICollection, StoreRepo>();


await DataSeed.SeedDataAsync(connectionString, builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
