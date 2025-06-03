using BlueBerry24.Application.Authorization.Handlers;
using BlueBerry24.Application.Config.Settings;
using BlueBerry24.Application.DI;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Infrastructure.Data;
using BlueBerry24.Infrastructure.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();



builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
                                  securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                                  {
                                      Name = "Authorization",
                                      Description = "Enter the token",
                                      In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                                      Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                                      Scheme = "Bearer"
                                  });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});







builder.Services.AddAutoMapper(typeof(BlueBerry24.Application.Mapping.AssemblyMarker));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:7105")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyConstants.SuperAdminOnly, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin));

    options.AddPolicy(PolicyConstants.AdminAndAbove, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin));

    options.AddPolicy(PolicyConstants.UserAndAbove, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin, RoleConstants.User));

    options.AddPolicy(PolicyConstants.AllRoles, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin, RoleConstants.User));

    // Specific permission policies
    options.AddPolicy(PolicyConstants.CanManageUsers, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin));

    options.AddPolicy(PolicyConstants.CanManageProducts, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin));

    options.AddPolicy(PolicyConstants.CanManageOrders, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin));

    options.AddPolicy(PolicyConstants.CanManageCoupons, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin));

    options.AddPolicy(PolicyConstants.CanViewReports, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin));

    options.AddPolicy(PolicyConstants.CanManageShops, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin));

    options.AddPolicy(PolicyConstants.CanManageInventory, policy =>
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin));
});

builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

JwtOptions jwtOptions = new JwtOptions();

//later
builder.Configuration.AddJsonFile("cartsettings.json", optional: true, reloadOnChange: true);
builder.Services.Configure<CartSettings>(builder.Configuration.GetSection("CartSettings"));


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Configuration.GetSection("ApiSettings:JwtOptions").Bind(jwtOptions);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
    };
});

builder.Services.AddAuthorization();


//builder.Services.AddScoped<IAuthRepository, AuthRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BlueBerry24 API");
        options.RoutePrefix = string.Empty;
    });
}

//var jwtOptions = app.Services.GetRequiredService<IOptions<JwtOptions>>().Value;

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
