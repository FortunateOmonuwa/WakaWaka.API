using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WakaWaka.API.DataAccessLayer.DataContext;
using WakaWaka.API.DataAccessLayer.Interfaces;
using WakaWaka.API.DataAccessLayer.Repository;
using WakaWaka.API.Domain.Models.restaurant;
using WakaWaka.API.Models.Hotel;
using WakaWaka.API.Models.Restaurant;
using WakaWaka.API.Models.Resturant;
using System.Text;
using WakaWaka.API.DataAccess.Interfaces;
using WakaWaka.API.DataAccess.Repository;
using WakaWaka.API.Service;

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
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"]
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
