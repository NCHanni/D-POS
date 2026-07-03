using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncBarcode
    {
        private const string LOCAL_TABLE = "item_barcode";
        private const string ENDPOINT_NAME = "Barcode";
        private const string ENDPOINT_DESC = "Item Barcode";

        private readonly Data.DBContext _context;
        public SyncBarcode(Data.DBContext context)
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

                int recordsAffected = 0;
                var list = new List<Models.Acumatica.Barcode>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.Barcode>>(result);
                }

                if (list.Any())
                {
                    try
                    {
                        foreach (Models.Acumatica.Barcode source in list)
                        {
                            var isNew = false;
                            var target = new Models.BusinessPro.item_barcode();
                            var sourceItemNo = source.InventoryCD.ToStringValue(30);
                            var sourceUOM = source.UOM.ToStringValue(30);
                            var sourceBarcode = source.AlternateID.ToStringValue(30);
                            var targets = _context.item_barcode.Where(c => c.barcode == sourceBarcode);

                            if (await targets.AnyAsync())
                                target = await targets.FirstAsync();
                            else
                            {
                                isNew = true;
                                target.id = 0;
                            }

                            target.item_no = sourceItemNo;
                            target.unit_of_measure = sourceUOM;
                            target.barcode = sourceBarcode;

                            if (isNew)
                                _context.item_barcode.Add(target);
                            else
                                _context.item_barcode.Update(target);

                            recordsAffected += await _context.SaveChangesAsync();
                        }

                        setting.timestamp = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
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
                        recordsAffected), 
                    _context);
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync(_context, ENDPOINT_DESC);
            }
        }
    }
}
