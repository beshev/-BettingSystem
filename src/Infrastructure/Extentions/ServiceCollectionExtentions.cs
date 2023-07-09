namespace Infrastructure.Extentions
{
    using AutoMapper;
    using Data;
    using Infrastructure.Mapping;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IMatchesService, MatchesService>();
            services.AddTransient<IBetsService, BetsService>();
            services.AddTransient<IOddsService, OddsService>();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<BettingMappingProfiler>();
            });

            services.AddSingleton(mappingConfig.CreateMapper());
            return services;
        }
    }
}
