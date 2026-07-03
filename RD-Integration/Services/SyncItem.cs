using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncItem
    {
        private const string LOCAL_TABLE = "item";
        private const string ENDPOINT_NAME = "Item";
        private const string ENDPOINT_DESC = "Item";

        private readonly Data.DBContext _context;
        public SyncItem(Data.DBContext context)
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
                var list = new List<Models.Acumatica.Item>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.Item>>(result);
                }

                if (list.Any())
                {
                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (Models.Acumatica.Item source in list)
                        {
                            var isNew = false;
                            var target = new Models.BusinessPro.item();
                            var targets = _context.item.Where(c => c.code == source.InventoryID.value);
                            var itemStatus = source.ItemStatus.ToStringValue();

                            //await Methods.LogAsync(source.InventoryID.value + "|" + itemStatus + "|" + isNew.ToString(), _context);

                            if (await targets.AnyAsync())
                                target = await targets.FirstAsync();
                            else
                            {
                                switch (itemStatus)
                                {
                                    case "Marked for Deletion":
                                    case "Inactive":
                                        continue; // Skip <---------
                                    default:
                                        isNew = true;
                                        break;
                                }
                            }

                            target.code = source.InventoryID.ToStringValue(30);
                            target.codename = source.InventoryID.ToStringValue(30);
                            target.sku = string.Empty;
                            target.description = source.Description.ToStringValue();
                            target.specifications = string.Empty;
                            target.class_code = string.Empty;
                            target.unit_of_measure = source.BaseUnit.ToStringValue(30);
                            target.price = source.DefaultPrice.ToDecimalValue();
                            target.is_vat = source.VAT.ToBooleanString();
                            target.is_zero_rated = source.Zerorated.ToBooleanString();
                            target.is_senior_pwd = source.SCPWDDiscount.ToBooleanString();
                            target.discount_group = source.Discount.ToStringValue(30);
                            target.allow_discount = string.IsNullOrWhiteSpace(source.Discount.ToStringValue()) ? false.ToString() : true.ToString();
                            target.inventory_posting_group = string.Empty;
                            target.vat_bus_posting_group = string.Empty;
                            target.gen_prod_posting_group = string.Empty;
                            target.vat_prod_posting_group = string.Empty;
                            target.is_active = true.ToString();
                            target.is_lot = source.TrackingMethod.value.Contains("Lot").ToString(); //source.Lot.ToBooleanString();
                            target.is_serial = source.TrackingMethod.value.Contains("Serial").ToString(); //source.Serial.ToBooleanString();

                            if (isNew)
                                _context.item.Add(target);
                            else
                            {
                                switch(itemStatus)
                                {
                                    case "Marked for Deletion":
                                    case "Active":
                                        target.is_active = true.ToString();
                                        break;
                                    case "Inactive":
                                        target.is_active = false.ToString();
                                        break;
                                }

                                _context.item.Update(target);

                                if (itemStatus == "Marked for Deletion")
                                {
                                    try
                                    {
                                        _context.item.Remove(target);
                                    }
                                    catch (Exception ex)
                                    {
                                        await ex.LogErrorAsync(_context, ENDPOINT_DESC);
                                    }
                                }
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
