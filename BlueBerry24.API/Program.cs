using BlueBerry24.API.OpenApi.Transformers;
using BlueBerry24.Application.Authorization.Handlers;
using BlueBerry24.Application.Config;
using BlueBerry24.Application.DI;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Infrastructure.Data;
using BlueBerry24.Infrastructure.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
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
builder.Services.AddOpenApi(options =>
{
    // Versioning config
    options.AddDocumentTransformer<VersionInfoTransformer>();

    // Security Scheme config
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
});



builder.Services.AddAutoMapper(typeof(BlueBerry24.Application.Mapping.AssemblyMarker));

builder.Services.AddApplicationServices(builder.Host, builder.Configuration);
builder.Services.AddInfrastructureServices();

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<BlueBerry24.Infrastructure.Data.DatabaseHealthCheck>("database");



builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<GmailSettings>(builder.Configuration.GetSection("GmailSettings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("App"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

JwtOptions jwtOptions = new JwtOptions();
builder.Configuration.GetSection("ApiSettings:JwtOptions").Bind(jwtOptions);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:7105", "http://host.docker.internal:3000", "http://frontend:30305")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});


builder.Services.AddProblemDetails();

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
    options.AddPolicy(PolicyConstants.CanManageUsers, policy => {
        policy.RequireRole(RoleConstants.SuperAdmin, RoleConstants.Admin);
        policy.RequireClaim("Permission", "ManageUsers");
    });

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


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "DefaultPolicy", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 100;
        limiterOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });
});

var app = builder.Build();

app.UseRateLimiter();

// Ensure database is created and updated with seeded data
// Ensure database is created and updated with seeded data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        Console.WriteLine("Starting database migration...");

        // Apply migrations
        context.Database.Migrate();

        Console.WriteLine("Database migration completed.");

        // Check if database is accessible
        var canConnect = await context.Database.CanConnectAsync();
        if (!canConnect)
        {
            Console.WriteLine("Cannot connect to database!");
            return;
        }

        Console.WriteLine("Database connection verified. Starting seeding...");

        // Seed initial data
        var dataSeeder = new BlueBerry24.Infrastructure.Data.DataSeeder(context, userManager, roleManager);
        await dataSeeder.SeedDataAsync();

        Console.WriteLine("All seeding operations completed successfully!");
    }
    catch (Exception ex)
    {
        // Log the error with full details
        Console.WriteLine($"An error occurred while migrating or seeding the database:");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Stack Trace: {ex.StackTrace}");

        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            Console.WriteLine($"Inner Stack Trace: {ex.InnerException.StackTrace}");
        }

        // Don't continue if seeding fails in development
        if (app.Environment.IsDevelopment())
        {
            throw;
        }
    }
}

if(app.Environment.IsDevelopment())
{
    // Configure the HTTP request pipeline.
    // Map OpenAPI endpoint
    app.MapOpenApi();

    // Map Scalar UI
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("BlueBerry24 API")
            .WithTheme(Scalar.AspNetCore.ScalarTheme.Purple)
            .WithDefaultHttpClient(Scalar.AspNetCore.ScalarTarget.CSharp, Scalar.AspNetCore.ScalarClient.HttpClient);
    });
}

//var jwtOptions = app.Services.GetRequiredService<IOptions<JwtOptions>>().Value;



// Only use HTTPS redirection in production when not in Docker
if (!app.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
}
else
{
    app.UseExceptionHandler("/error-development");
}

    //app.UseHsts();


 
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

// Add health check endpoint
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseSerilogRequestLogging();



app.MapControllers();

app.Run();
