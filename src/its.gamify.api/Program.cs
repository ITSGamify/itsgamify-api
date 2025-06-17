using its.gamify.api;
using its.gamify.core.Mappers;
using its.gamify.domains.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCoreServices();
builder.Services.AddControllers();
builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Configuration.AddUserSecrets<Program>();
var configuration = builder.Configuration
    .Get<AppSetting>() ?? throw new Exception("Null configuration");

builder.Services.AddSingleton(configuration);
builder.Services.AddOpenApi();
builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperConfigurationProfile).Assembly);
var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
app.MapOpenApi();



app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
