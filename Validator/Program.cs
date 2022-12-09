using Shared;
using Validator;
using Validator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var settings = builder.Configuration.Get<AppSettings>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(settings);
builder.Services.AddSingleton(new FileStorageService(settings.FileStorageUrl));
builder.Services.AddSingleton(new DataHandlerService(settings.DataHandlerUrl));
builder.Services.AddSingleton<ValidationService>();
builder.Services.AddHostedService<BackgroundQueueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();