using its.gamify.core.Mappers;
using its.gamify.infras.Datas;
using Microsoft.EntityFrameworkCore;
using Scrutor;
using System.Reflection;

namespace its.gamify.api;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Core Services
    /// Todo: Swagger Generation
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            services.BuildServiceProvider().GetRequiredService<IConfiguration>()
                .GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
        // services.AddDbContext<AppDbContext>(options => options.USeSqlServer(
        //     services.BuildServiceProvider().GetRequiredService<IConfiguration>()
        //         .GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

        services.AddHttpContextAccessor();

        services.Scan(scan =>
        {
            scan.FromAssemblies(getAssemblies())
                .AddClasses()
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime();
        });
        // AutoMapper
        services.AddAutoMapper(typeof(MapperConfigurationProfile));
        return services;
    }
    private static Assembly[] getAssemblies()
        => [AssemblyReference.Assembly, infras.AssemblyReference.Assembly, core.AssemblyReference.Assembly];

}