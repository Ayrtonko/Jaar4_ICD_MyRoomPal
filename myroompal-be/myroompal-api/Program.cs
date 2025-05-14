using Microsoft.EntityFrameworkCore;
using myroompal_api.Data;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using System.Text.Json.Serialization;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using myroompal_api.Modules.MatchManagement.Interfaces;
using myroompal_api.Modules.MatchManagement.Repositories;
using myroompal_api.Modules.MatchManagement.Services;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.Support.Interfaces;
using myroompal_api.Modules.Support.Repositories;
using myroompal_api.Modules.Support.Services;
using myroompal_api.Modules.UserManagement.Interfaces;
using myroompal_api.Modules.UserManagement.Repositories;
using myroompal_api.Modules.UserManagement.Services;
using myroompal_api.Modules.ProfileManagement.Models;
using myroompal_api.Modules.RoomManagement.Interfaces;
using myroompal_api.Modules.RoomManagement.Repositories;
using myroompal_api.Modules.RoomManagement.Services;

var builder = WebApplication.CreateBuilder(args);
string? appEnvironment = Environment.GetEnvironmentVariable("APP_ENVIRONMENT");

SecretClientOptions options = new SecretClientOptions()
{
    Retry =
    {
        Delay = TimeSpan.FromSeconds(2),
        MaxDelay = TimeSpan.FromSeconds(16),
        MaxRetries = 5,
        Mode = RetryMode.Exponential
    }
};

var client = new SecretClient(new Uri("https://myroompal-kv.vault.azure.net"), new DefaultAzureCredential(), options);

try
{
    // Fetch secrets
    KeyVaultSecret auth0DomainSecret = client.GetSecret($"Auth0-Domain{appEnvironment}");
    KeyVaultSecret auth0AudienceSecret = client.GetSecret($"Auth0-Audience{appEnvironment}");

    builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
    {
        { $"Auth0-Domain", auth0DomainSecret.Value },
        { $"Auth0-Audience", auth0AudienceSecret.Value }
    });
}
catch (Exception ex)
{
    Console.WriteLine($"Error retrieving secrets from Key Vault: {ex.Message}");
    throw;
}

// Set app settings values
var authDomain = builder.Configuration[$"Auth0-Domain"];
var authAudience = builder.Configuration[$"Auth0-Audience"];
if (authDomain == null || authAudience == null)
{
    throw new Exception("Auth0 domain or audience not found in configuration");
}
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
if(appEnvironment == "tst" || appEnvironment == "prd")
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor();
}
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{authDomain}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = $"https://{authDomain}/",
            ValidAudiences = new[] { authAudience },
            NameClaimType = "sub"
        };
    });

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Register Service interfaces and their implementations
builder.Services.AddScoped<ISupportService, SupportService>();
builder.Services.AddScoped<ISupportRepository, SupportRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMatchingService, MatchingService>();
builder.Services.AddScoped<IMatchingRepository, MatchingRepository>();
builder.Services.AddScoped<IAuth0Context, Auth0Context>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();


// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder
            .WithOrigins("https://salmon-bush-02fe80a03.5.azurestaticapps.net",
                "https://white-bush-02a09c303.5.azurestaticapps.net",
                "http://localhost:3000", authDomain)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Configure the database connection
if (appEnvironment == "tst")
{
    KeyVaultSecret dbConnectionStringSecret = client.GetSecret("sqlConnectionStringKey-tst");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(dbConnectionStringSecret.Value)
    );
    
}
else if (appEnvironment == "prd")
{
    KeyVaultSecret dbConnectionStringSecret = client.GetSecret("sqlConnectionStringKey-prd");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(dbConnectionStringSecret.Value)
    );
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || appEnvironment == "tst")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    bool isProduction = appEnvironment == "prd";
    DbInitializer.Seed(context, isProduction);
}
app.Run();
