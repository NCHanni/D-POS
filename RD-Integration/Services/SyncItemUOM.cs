using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncItemUOM
    {
        private const string LOCAL_TABLE = "item_unit_of_measure";
        private const string ENDPOINT_NAME = "ItemUOM";
        private const string ENDPOINT_DESC = "Item UOM";

        private readonly Data.DBContext _context;
        public SyncItemUOM(Data.DBContext context)
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

                var list = new List<Models.Acumatica.ItemUOM>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.ItemUOM>>(result);
                }

                if (list.Any())
                {
                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (Models.Acumatica.ItemUOM source in list)
                        {
                            var isNew = false;
                            var target = new Models.BusinessPro.item_unit_of_measure();
                            var sourceItemNo = source.InventoryCD.ToStringValue(30);
                            var sourceUOM = source.FromUnit.ToStringValue(30);
                            var targets = _context.item_unit_of_measure.Where(c => c.item_no == sourceItemNo && c.unit_of_measure == sourceUOM);

                            if (await targets.AnyAsync())
                                target = await targets.FirstAsync();
                            else
                            {
                                isNew = true;
                                target.id = 0;
                            }

                            target.item_no = sourceItemNo;
                            target.unit_of_measure = sourceUOM;
                            target.qty_per_uom = source.ConversionFactor.ToDecimalValue();
                            target.to_uom = source.ToUnit.ToStringValue(30);

                            if (isNew)
                                _context.item_unit_of_measure.Add(target);
                            else
                                _context.item_unit_of_measure.Update(target);
                        }

                        setting.timestamp = DateTime.Now;
                        await _context.SaveChangesAsync();
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
                        list.Count()), 
                    _context);
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync(_context, ENDPOINT_DESC);
            }
        }
    }
}
