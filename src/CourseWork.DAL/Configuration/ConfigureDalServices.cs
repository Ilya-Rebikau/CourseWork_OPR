using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CourseWork.DAL.Configuration
{
    public static class ConfigureDalServices
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services)
        {
            services.AddScoped<ISerializer, CsvSerializer>();
            return services;
        }
    }
}
