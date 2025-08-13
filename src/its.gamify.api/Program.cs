using its.gamify.api;
using its.gamify.api.Middlewares;
using its.gamify.core.Features.AvailablesData;
using its.gamify.core.Mappers;
using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new OrderParamListBinderProvider());
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
});
// Add services to the container.
var configuration = builder.Configuration
    .Get<AppSetting>() ?? throw new Exception("Null configuration");
builder.Services.AddCoreServices(appSetting: configuration);


builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddSingleton<GlobalErrorHandlingMiddleware>();
builder.Services.AddSingleton<Ultils>();
builder.Services.AddSingleton(configuration);
builder.Services.AddOpenApi();
builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperConfigurationProfile).Assembly);
var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseCors("AllowAll");
app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
app.MapOpenApi();



app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
