using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RealEstateAgency.Api;
using RealEstateAgency.Api.Services;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;
using RealEstateAgency.Domain.Repository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IRepository<Client, int>, Repository<Client, int>>();
builder.Services.AddScoped<IRepository<Order, int>, Repository<Order, int>>();
builder.Services.AddScoped<IRepository<RealEstate, int>, Repository<RealEstate, int>>();

builder.Services.AddTransient<ClientService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<RealEstateService>();
builder.Services.AddTransient<AnalyticsService>();

builder.Services.AddDbContext<RealEstateAgencyContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Postgre"),
        new MySqlServerVersion(new Version(8, 0, 23))));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Client API",
        Version = "v1",
        Description = "API для работы с клиентами"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client API v1");
    });
}

app.UseRouting();

app.MapControllers();

app.Run();
