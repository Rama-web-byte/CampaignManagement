using CampaignManagement.Data;
using Microsoft.EntityFrameworkCore;
using CampaignManagement.Mapper;
using CampaignManagement.Repositories.Contracts;
using CampaignManagement.Repositories.Implementations;
using CampaignManagement.Services.Contracts;
using CampaignManagement.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CampaignManagement.Validator;
using FluentValidation;
using FluentValidation.AspNetCore;
using CampaignManagement.Models;
using System.IO;
using Microsoft.AspNetCore.ResponseCompression;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using CampaignManagement.Telemetry;
using CampaignManagement.Middleware;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
//Application Insights

builder.Services.AddOpenTelemetry().UseAzureMonitor
    (options=>
    { options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    });

//DB
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
//var connectionString = builder.Configuration.GetConnectionString("DB_CONNECTION_STRING");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddValidatorsFromAssemblyContaining<CampaignValidator>();
builder.Services.AddScoped<IValidator<User>,UserValidator>();
builder.Services.AddFluentValidationAutoValidation();

//Register DI
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSingleton<ITelemetryInitializer, JwtUserTelemetryInitializer>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

//
builder.Services.AddResponseCompression(option =>
{
    option.EnableForHttps = true;
    option.Providers.Add<BrotliCompressionProvider>();
    option.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Optimal;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Optimal;

});

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            //   policy.WithOrigins("http://localhost:3000") // React frontend running locally
            policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
        });
});

//Swagger

builder.Services.AddEndpointsApiExplorer();

// Make swagger to remember the jwt auth
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token with 'Bearer ' prefix",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
    {
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Reference = new Microsoft.OpenApi.Models.OpenApiReference
            {
                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }});
});

//JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddRateLimiter(options =>
{
    //options.AddPolicy("LoginPolicy", context => RateLimitPartition.GetIpAddressLimiter
    //(context, ip =>
    //new FixedWindowRateLimiterOptions
    //{
    //    PermitLimit = 5,
    //    Window = TimeSpan.FromMinutes(1),
    //    QueueLimit = 0,
    //    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
    //}));
    options.GlobalLimiter=PartitionedRateLimiter.Create<HttpContext,string>(context=>
    {
        var userId = context.User?.FindFirst("UserId")?.Value ?? "anonymous";


        return RateLimitPartition.GetTokenBucketLimiter(userId, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 100,
            TokensPerPeriod = 100,
            ReplenishmentPeriod = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    });
    options.AddPolicy("WritePolicy", context =>
    {
        var userId = context.User?.FindFirst("UserId")?.Value ?? "anonymous";
        return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit=20,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            QueueProcessingOrder= QueueProcessingOrder.OldestFirst
        });
    });
    options.RejectionStatusCode = 429;
});
var app = builder.Build();

using(var scope=app.Services.CreateScope())
{
    var dbContext=scope.ServiceProvider.GetService<AppDbContext>();
    DbSeeder.SeedUsers(dbContext);
}

app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campaign Management API v1");
    });
}

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseMiddleware<JWTUserMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();


app.Run();
