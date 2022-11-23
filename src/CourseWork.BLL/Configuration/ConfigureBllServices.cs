using CourseWork.BLL.Interfaces;
using CourseWork.BLL.Services;
using CourseWork.DAL.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseWork.BLL.Configuration
{
    public static class ConfigureBllServices
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddDalServices();
            services.AddScoped<ISolver, BranchesAndBoundariesSolver>();
            return services;
        }
    }
}
