using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncItemSalePrice
    {
        private const string LOCAL_TABLE = "item_sale_price";
        private const string ENDPOINT_NAME = "ItemSalePrice";
        private const string ENDPOINT_DESC = "Item Sale Price";

        private readonly Data.DBContext _context;
        public SyncItemSalePrice(Data.DBContext context)
        {
            _context = context;
        }

        public async Task Run(bool manual = false)
        {
            DateTimeOffset startRunTime = DateTimeOffset.Now;
            Models.RD_Preference preference = await _context.rd_preference.FirstOrDefaultAsync();
            Models.BusinessPro.settings_sync setting = await _context.settings_sync.Where(w => w.table_name == LOCAL_TABLE).FirstAsync();

            try
            {
                int numAttempts = 0;
                string parameters = Methods.GetTimestampParameter(setting.timestamp, preference.TimestampDateOffset);
                string result = String.Empty;
                bool hasError = false;

                using (REST rs = new REST(preference))
                {
                    do
                    {
                        try
                        {
                            numAttempts++;
                            hasError = false;
                            result = await rs.GetAsync(ENDPOINT_NAME, parameters);
                        }
                        catch (Exception ex)
                        {
                            if (numAttempts > Constants.MAX_RETRY_ATTEMPTS)
                                throw;
                            else if (ex.IsConnectionError())
                            {
                                hasError = true;
                                await Task.Delay(Constants.RETRY_INTERVAL);
                            }
                            else throw;
                        }
                    }
                    while (hasError);
                }

                var rowsAffected = 0;
                var list = new List<Models.Acumatica.ItemSalePrice>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.ItemSalePrice>>(result);
                }

                if (list.Any())
                {
                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        var ctr = 0;
                        foreach (Models.Acumatica.ItemSalePrice source in list)
                        {
                            ctr++;

                            var isNew = false;
                            var target = new Models.BusinessPro.item_sale_price();
                            var targets = _context.item_sale_price.Where(p =>
                                p.sales_type == source.PriceType.value &&
                                p.alternate_id == source.AlternateID.value &&
                                p.item_no == source.InventoryID.value &&
                                p.unit_of_measure_code == source.UOM.value);

                            if (await targets.AnyAsync())
                                target = await targets.FirstAsync();
                            else
                            {
                                isNew = true;
                                target.id = 0;
                            }

                            target.alternate_id = source.AlternateID.ToStringValue(30);
                            target.sales_type = source.PriceType.ToStringValue(30);
                            target.sales_code = source.ReferenceNbr.ToStringValue(30);
                            target.item_no = source.InventoryID.ToStringValue(30);
                            target.unit_of_measure_code = source.UOM.ToStringValue(30);
                            target.minimum_quantity = Math.Round(source.BreakQty.ToDecimalValue(), 2);
                            target.starting_date = source.EffectiveDate.ToDateTimeValue();
                            target.ending_date = source.ExpirationDate.ToDateTimeValue();
                            target.unit_price = Math.Round(source.PendingPrice.ToDecimalValue(), 2);
                            target.price_includes_vat = true;

                            if (target.unit_price < 1000000) //less than 1 million
                            {
                                if (isNew)
                                    _context.item_sale_price.Add(target);
                                else
                                    _context.item_sale_price.Update(target);
                            }

                            if (ctr >= 100) //save and commit every hundred records
                            {
                                setting.timestamp = DateTime.Now;
                                rowsAffected += await _context.SaveChangesAsync();
                                ctr = 0;
                            }
                        }

                        setting.timestamp = DateTime.Now;
                        rowsAffected += await _context.SaveChangesAsync();
                        await scope.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await scope.RollbackAsync();
                        await ex.LogErrorAsync(_context, ENDPOINT_DESC);
                    }
                }
                else
                {
                    setting.timestamp = DateTime.Now;
                }

                await Methods.LogAsync(
                    String.Format(
                        Messages.RunFormat, 
                        manual ? Messages.Manual : Messages.Scheduled, 
                        ENDPOINT_DESC,
                        (int)(DateTimeOffset.Now - startRunTime).TotalSeconds,
                        rowsAffected), 
                    _context);
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync(_context, ENDPOINT_DESC);
            }
        }
    }
}
