using CourseWork.BLL.Interfaces;
using CourseWork.BLL.Services;
using CourseWork.DAL.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseWork.BLL.Configuration
{
    /// <summary>
    /// Конфигурация сервисов бизнес-логики.
    /// </summary>
    public static class ConfigureBllServices
    {
        /// <summary>
        /// Добавляет сервисы бизнес-логики в IServiceCollection.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <returns>Добавленные сервисы.</returns>
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddDalServices();
            services.AddScoped<ISolver, BranchesAndBoundariesSolver>();
            return services;
        }
    }
}
