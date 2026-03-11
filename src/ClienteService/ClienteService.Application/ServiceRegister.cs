using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ClienteService.Application
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationService(
            this IServiceCollection services)
        {
            AddServiceToDiContainer(services);

            return services;
        }

        private static IServiceCollection AddServiceToDiContainer(
            this IServiceCollection services)
        {
            var applicationAssembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(applicationAssembly);

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(applicationAssembly);
            });

            return services;
        }
    }
}
