using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Application
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
