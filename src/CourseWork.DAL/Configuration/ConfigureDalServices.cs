using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CourseWork.DAL.Configuration
{
    /// <summary>
    /// Конфигурация сервисов из слоя доступа к данным.
    /// </summary>
    public static class ConfigureDalServices
    {
        /// <summary>
        /// Добавляет сервисы из слоя доступа к данным в IServiceCollection.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <returns>Добавленные сервисы.</returns>
        public static IServiceCollection AddDalServices(this IServiceCollection services)
        {
            services.AddScoped<ISerializer, CsvSerializer>();
            return services;
        }
    }
}
