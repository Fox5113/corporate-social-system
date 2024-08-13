using DataAccess.Context;
using DataAccess.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Indentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

Configuration.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configuration.ConfigureMiddleware(app);

app.Run();
