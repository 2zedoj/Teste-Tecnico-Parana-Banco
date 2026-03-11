using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaoService.Worker.Workers
{
    public class CartaoWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CartaoWorker> _logger;

        public CartaoWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<CartaoWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CartaoWorker iniciado, aguardando mensagens...");
            await Task.Delay(Timeout.Infinite, stoppingToken);
            // MassTransit gerencia o loop de consumo — não precisa de while aqui
        }
    }
}
