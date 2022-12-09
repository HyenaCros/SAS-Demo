using DataHandler.DataLayer;
using DataHandler.Profiles;
using DataHandler.Profiles.Services;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var settings = builder.Configuration.Get<AppSettings>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkInMemoryDatabase();
builder.Services.AddAutoMapper(typeof(DataProfiles));
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseInMemoryDatabase("Data");
});
builder.Services.AddSingleton(new FileStorageService(settings.FileStorageUrl));
builder.Services.AddSingleton(new ValidationService(settings.ValidatorUrl));
builder.Services.AddScoped<UploadsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config => config.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();