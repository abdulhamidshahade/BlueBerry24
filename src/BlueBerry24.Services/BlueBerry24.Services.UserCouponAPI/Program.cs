using BlueBerry24.Services.UserCouponAPI.Data;
using BlueBerry24.Services.UserCouponAPI.Messaging.Server;
using BlueBerry24.Services.UserCouponAPI.Services;
using BlueBerry24.Services.UserCouponAPI.Services.Interfaces;
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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddHttpClient("Coupon", c => c.BaseAddress =
new Uri(builder.Configuration["ApiSettings:CouponAPIUrl"]));

builder.Services.AddHttpClient("User", c => c.BaseAddress =
new Uri(builder.Configuration["ApiSettings:UserAPIUrl"]));


//builder.Services.AddScoped<IUserCouponService, UserCouponService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddHostedService<UserCouponRpcServer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "UserCouponAPI v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
