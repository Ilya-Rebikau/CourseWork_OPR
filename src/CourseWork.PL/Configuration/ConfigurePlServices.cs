using CourseWork.BLL.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseWork.PL.Configuration
{
    public static class ConfigurePlServices
    {
        public static IServiceCollection AddPlServices(this IServiceCollection services)
        {
            services.AddBllServices();
            services.AddTransient<MainForm>();
            return services;
        }
    }
}
