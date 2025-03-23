using BlueBerry24.Services.StockAPI.Data;
using BlueBerry24.Services.StockAPI.Services;
using BlueBerry24.Services.StockAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLServer"));
});

builder.Services.AddHttpClient("Product", options => options.BaseAddress =
new Uri(builder.Configuration["ApiUrls:ProductAPI"]));

builder.Services.AddHttpClient("Shop", options => options.BaseAddress =
new Uri(builder.Configuration["ApiUrls:ShopAPI"]));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "StockAPI v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
