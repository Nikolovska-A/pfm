﻿using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Npgsql;
using PFMBackendAPI.Database;
using PFMBackendAPI.Database.Repositories;
using PFMBackendAPI.Helpers;
using PFMBackendAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISplitService, SplitService>();
builder.Services.AddScoped<ISplitRepository, SplitRepository>();
//AutoMapper definition
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = ErrorResponseValidator.MakeValidationResponse;
});
builder.Services
    .AddMvc()
    // Or .AddControllers(...)
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DBContext registration
builder.Services.AddDbContext<FinanceDbContext>(opt =>
{
    opt.UseNpgsql(CreateConnectionString(builder.Configuration));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
    scope.ServiceProvider.GetRequiredService<FinanceDbContext>().Database.Migrate();

    app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

    app.UseHttpsRedirection();
   

}

app.UseAuthorization();

app.MapControllers();

app.Run();

string CreateConnectionString(IConfiguration configuration)
{
    var username = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "postgres";
    var pass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "admin";
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "finance";
    var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "docker.for.mac.host.internal";
    var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";

    var connBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = host,
        Port = int.Parse(port),
        Username = username,
        Database= databaseName,
        Password = pass,
        Pooling = true

    };

    return connBuilder.ConnectionString;
}

