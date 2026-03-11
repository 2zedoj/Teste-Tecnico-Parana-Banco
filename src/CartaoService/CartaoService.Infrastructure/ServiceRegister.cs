using CartaoService.Domain.Abstraction;
using CartaoService.Infrastructure.Repositories;
using CartaoService.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using CartaoService.Infrastructure.Messaging;


namespace CartaoService.Infrastructure
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
                x.AddConsumer<EmitirCartoesConsumer>();

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

            return services;
        }
    }
}
