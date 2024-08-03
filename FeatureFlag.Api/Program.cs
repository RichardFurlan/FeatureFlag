using FeatureFlag.Application.Consumidores;
using FeatureFlag.Application.Factory;
using FeatureFlag.Application.Recursos;
using FeatureFlag.Application.RecursosConsumidores;
using FeatureFlag.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository.Persistence;
using Repository.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dataBase  = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<FeatureFlagDbContext>(options =>
    options.UseNpgsql(dataBase));

builder.Services.AddScoped<IRepConsumidor, RepConsumidor>();
builder.Services.AddScoped<IRepRecurso, RepRecurso>();
builder.Services.AddScoped<IRepRecursoConsumidor, RepRecursoConsumidor>();

builder.Services.AddScoped<IAplicConsumidor, AplicConsumidor>();
builder.Services.AddScoped<IAplicRecurso, AplicRecurso>();
builder.Services.AddScoped<IAplicRecursoConsumidor, AplicRecursoConsumidor>();
builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisOutputCache(
    options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
        options.InstanceName = "FeatureFlagCache";
    });
builder.Services.AddOutputCache();

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

app.UseOutputCache();

app.Run();