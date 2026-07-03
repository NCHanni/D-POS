using Cronos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class ScheduleHostedService : BackgroundService
    {
        protected IServiceProvider _serviceProvider;

        public ScheduleHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DoWork();
                await Task.Delay(new TimeSpan(0, 0, 30));
            }
        }

        private async void DoWork()
        {
            using var scope = _serviceProvider.CreateScope();
            Data.DBContext context = scope.ServiceProvider.GetRequiredService<Data.DBContext>();

            try
            {
                var preference = await context.rd_preference.FirstOrDefaultAsync();

                if (!preference.EnableSchedule) { return; }

                List<Models.BusinessPro.settings_sync> tables = 
                    await context.settings_sync.Where(w => w.enabled == true && !string.IsNullOrEmpty(w.cron) && w.rd_integration).ToListAsync();

                var timestamp = DateTimeOffset.Now;

                foreach (Models.BusinessPro.settings_sync t in tables)
                {
                    try
                    {
                        CronExpression expression = CronExpression.Parse(t.cron);

                        var next = expression.GetNextOccurrence(timestamp, TimeZoneInfo.Local);

                        if (next.HasValue)
                        {
                            if (next.Value <= timestamp.AddSeconds(30))
                            {
                                switch (t.table_name)
                                {
                                    case "company_information":
                                        await new SyncCompany(context).Run();
                                        break;
                                    case "credit_card":
                                        await new SyncCreditCard(context).Run();
                                        break;
                                    case "credit_memo":
                                        await new SyncCreditMemo(context).Run();
                                        break;
                                    case "customer":
                                        await new SyncCustomer(context).Run();
                                        break;
                                    case "location":
                                        await new SyncLocation(context).Run();
                                        break;
                                    case "gift_certificate":
                                        await new SyncGiftCert(context).Run();
                                        break;
                                    case "salesperson":
                                        await new SyncSalesperson(context).Run();
                                        break;
                                    case "unit_of_measure":
                                        await new SyncUOM(context).Run();
                                        break;
                                    case "discount_group":
                                        await new SyncDiscount(context).Run();
                                        break;
                                    case "item_class":
                                        await new SyncItemClass(context).Run();
                                        break;
                                    case "item_sale_price":
                                        await new SyncItemSalePrice(context).Run();
                                        break;
                                    case "item":
                                        await new SyncItem(context).Run();
                                        break;
                                    case "item_barcode":
                                        await new SyncBarcode(context).Run();
                                        break;
                                    case "item_unit_of_measure":
                                        await new SyncItemUOM(context).Run();
                                        break;
                                    case "sale":
                                        await new SyncSale(context).Run();
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await ex.LogErrorAsync(context, string.Format("Schedule-{0}", t.table_name));
                    }
                }
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync(context, "Schedule-DoWork");
            }
        }
    }
}
