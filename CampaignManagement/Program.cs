using CampaignManagement.Data;
using Microsoft.EntityFrameworkCore;
using CampaignManagement.Mapper;
using CampaignManagement.Repositories.Contracts;
using CampaignManagement.Repositories.Implementations;
using CampaignManagement.Services.Contracts;
using CampaignManagement.Services.Implementations;


var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
//var connectionString = builder.Configuration.GetConnectionString("DB_CONNECTION_STRING");


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // React frontend running locally
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();           
var app = builder.Build();

app.UseCors("AllowSpecificOrigins");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campaign Management API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();


app.Run();
