using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Antomi.Services
{
    public class DiscountBackgroundService : BackgroundService
    {
        private readonly IServiceProvider Service;

        public DiscountBackgroundService(IServiceProvider serviceScopeFactory)
        {
            //   this.discount = discount;
            this.Service = serviceScopeFactory;
        }

       
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = Service.CreateScope())
                {
                    var scopeDiscountDelete = scope.ServiceProvider.GetRequiredService<IDiscountDelete>();
                    await scopeDiscountDelete.Delete();
                }
               await  StopAsync(stoppingToken);
                break;
            }
            await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
