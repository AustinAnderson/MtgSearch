using Microsoft.Extensions.Logging.Configuration;
using MtgSearch.Server;
using MtgSearch.Server.Models.Logic;
using MtgSearch.Server.Models.Logic.Highlighting;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "wwwroot/browser",
    Args = args
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(l=>
{
    //just console for now
    l.AddConsole();
});
builder.Services.AddSingleton<ITextMarker, TextMarker>();
builder.Services.AddSingleton<ICardRepository, ScryfallCardRepository>();
//initializes the repo on server start up rather than first request
builder.Services.AddHostedService<StartupBackgroundService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
