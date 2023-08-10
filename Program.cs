using Microsoft.EntityFrameworkCore;
using WakaWaka.API.DataAccessLayer.DataContext;
using WakaWaka.API.DataAccessLayer.Interfaces;
using WakaWaka.API.DataAccessLayer.Repository;
using WakaWaka.API.Domain.Models.Hotel;
using WakaWaka.API.Models.Hotel;
using WakaWaka.API.Models.Restaurant;
using WakaWaka.API.Models.Resturant;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("WakaWaka");
builder.Services.AddDbContext<WakaContext>(options => options.UseSqlServer(connection));
builder.Services.AddTransient<IBaseRepository<Hotel>, HotelRepository>();
builder.Services.AddTransient<IReviewRepository<HotelReview>, HotelRepository>();
builder.Services.AddTransient<IBaseRepository<Restaurant>, RestaurantRepository>();
builder.Services.AddTransient<IReviewRepository<RestaurantReview>, RestaurantRepository>();
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
