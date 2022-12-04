using CourseWork.BLL.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseWork.PL.Configuration
{
    /// <summary>
    /// Конфигурация сервисов для слоя представления.
    /// </summary>
    public static class ConfigurePlServices
    {
        /// <summary>
        /// Добавляет сервисы из слоя представления в IServiceCollection.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <returns>Добавленные сервисы.</returns>
        public static IServiceCollection AddPlServices(this IServiceCollection services)
        {
            services.AddBllServices();
            services.AddTransient<MainForm>();
            return services;
        }
    }
}
