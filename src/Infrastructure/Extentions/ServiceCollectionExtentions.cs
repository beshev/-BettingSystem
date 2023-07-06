namespace Infrastructure.Extentions
{
    using Data;
    using AutoMapper;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.Mapping;

    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ISportsService, SportsService>();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<SportsMappingProfiler>();
            });

            services.AddSingleton(mappingConfig.CreateMapper());
            return services;
        }
    }
}
