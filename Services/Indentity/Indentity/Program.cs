using DataAccess.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Indentity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

Configuration.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configuration.ConfigureMiddleware(app);

app.Run();
