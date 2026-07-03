using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncItemClass
    {
        private const string LOCAL_TABLE = "item_class";
        private const string ENDPOINT_NAME = "ItemClass";
        private const string ENDPOINT_DESC = "Item Class";

        private readonly Data.DBContext _context;
        public SyncItemClass(Data.DBContext context)
        {
            _context = context;
        }

        public async Task Run(bool manual = false)
        {
            DateTimeOffset startRunTime = DateTimeOffset.Now;
            Models.RD_Preference preference = _context.rd_preference.FirstOrDefault();
            Models.BusinessPro.settings_sync setting = _context.settings_sync.Where(w => w.table_name == LOCAL_TABLE).First();

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

                var list = new List<Models.Acumatica.ItemClass>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.ItemClass>>(result);
                }

                if (list.Any())
                {
                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (Models.Acumatica.ItemClass source in list)
                        {
                            var isNew = false;
                            var target = new Models.BusinessPro.item_class();
                            var targets = _context.item_class.Where(c => c.code == source.ClassID.value);

                            if (await targets.AnyAsync())
                                target = await targets.FirstAsync();
                            else
                            {
                                isNew = true;
                            }

                            target.code = source.ClassID.ToStringValue(30);
                            target.codename = source.ClassID.ToStringValue(50);
                            target.description = source.Description.ToStringValue();

                            if (isNew)
                                _context.item_class.Add(target);
                            else
                                _context.item_class.Update(target);
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
