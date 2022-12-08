using System.Text.Json;
using FileWatcher;
using FileWatcher.Services;
using Rebex.Net;
using Shared;

var builder = WebApplication.CreateBuilder(args);

Rebex.Licensing.Key = "==AWmROWYpZzP2uPK0SJixqkcl105qu0yqNOd7E6+GvSdM==";

// Add services to the container.
var settings = builder.Configuration.Get<AppSettings>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddSingleton(settings);
builder.Services.AddSingleton(new Sftp());
builder.Services.AddSingleton(new DataHandlerService(settings.DataHandlerUrl));
builder.Services.AddSingleton(new FileStorageService(settings.FileStorageUrl));
builder.Services.AddSingleton<ValidationService>();
builder.Services.AddSingleton<FtpService>();
builder.Services.AddHostedService<BackgroundPollingService>();
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