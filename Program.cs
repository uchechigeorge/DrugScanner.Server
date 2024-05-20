using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DrugScanner.Server;
using DrugScanner.Server.Models;
using DrugScanner.Server.Services;

var builder = WebApplication.CreateBuilder(args);
// Set configuration
IoCContainer.Configuration = builder.Configuration;

// Add services to the container.
ServiceModule.RegisterServices(builder.Services);

builder.Services.AddControllers()
                .AddNewtonsoftJson(s =>
                {
                  s.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  s.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

builder.Services.AddCors();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseSqlServer(IoCContainer.Configuration.GetConnectionString("Default"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
IoCContainer.Environment = app.Environment;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(
  options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
  //.AllowCredentials()
  );

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.Run();