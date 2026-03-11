using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropostaService.Domain.Abstraction;
using PropostaService.Infrastructure.Messaging;
using PropostaService.Infrastructure.Repositories;
using PropostaService.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropostaService.Infrastructure
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddInfrastructureService(
            this IServiceCollection services,
            IConfiguration config)
        {
            AddDbConnection(services, config);

            AddServiceToDiContainer(services);

            return services;
        }

        private static IServiceCollection AddDbConnection(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("Database"));
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GerarPropostaConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(config["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(config["RabbitMQ:Username"]);
                        h.Password(config["RabbitMQ:Password"]);
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
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
    }
}
