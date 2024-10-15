using Microsoft.OpenApi.Models;
using RealEstateAgency.Api;
using RealEstateAgency.Api.Services;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;
using RealEstateAgency.Domain.Repository.Mock;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddTransient<IRepository<Client, int>, ClientRepositoryMock>();
builder.Services.AddTransient<IRepository<Order, int>, OrderRepository>();
builder.Services.AddTransient<IRepository<RealEstate, int>, RealEstateRepository>();


builder.Services.AddTransient<ClientService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<RealEstateService>();


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
