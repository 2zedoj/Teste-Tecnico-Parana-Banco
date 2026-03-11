using ClienteService.Domain.Abstraction;
using ClienteService.Domain.Common.Events;
using ClienteService.Infrastructure.Messaging;
using ClienteService.Infrastructure.Repositories;
using ClienteService.Infrastructure.UnitOfWorks;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClienteService.Infrastructure
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddInfrastructureService(
            this IServiceCollection services,
            IConfiguration config)
        {
            AddDbConnection(services, config);

            AddServiceToDiContainer(services);

            AddMassTransit(services, config);

            return services;
        }

        private static IServiceCollection AddDbConnection(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("Database"));
            });
            return services;
        }

        private static IServiceCollection AddServiceToDiContainer(
            this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

            return services;
        }

        private static IServiceCollection AddMassTransit(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.UseMessageRetry(r =>
                        r.Exponential(5,
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(30),
                            TimeSpan.FromSeconds(5)));

                    cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });
                });
            });

            return services;
        }
    }
}
