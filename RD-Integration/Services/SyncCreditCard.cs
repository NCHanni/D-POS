using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncCreditCard
    {
        private const string LOCAL_TABLE = "credit_card";
        private const string ENDPOINT_NAME = "CreditCard";
        private const string ENDPOINT_DESC = "Credit Card";

        private readonly Data.DBContext _context;
        public SyncCreditCard(Data.DBContext context)
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

                var list = new List<Models.Acumatica.CreditCard>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.CreditCard>>(result);
                }

                if (list.Any())
                {
                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (Models.Acumatica.CreditCard source in list)
                        {
                            var isNew = false;
                            var target = new Models.BusinessPro.credit_card();
                            var targets = _context.credit_card.Where(c => c.code == source.Code.value);

                            if (await targets.AnyAsync())
                                target = await targets.FirstAsync();
                            else
                                isNew = true;

                            target.code = source.Code.ToStringValue(30);
                            target.description = source.Description.ToStringValue();
                            target.bank_name = source.BankName.ToStringValue(30);
                            target.account_type = string.Empty; //source.AccountType.ToStringValue();
                            target.account_no = string.Empty; //source.AccountNo.ToDateTimeValue();
                            target.is_active = source.Active.ToBooleanString();

                            if (isNew)
                                _context.credit_card.Add(target);
                            else
                                _context.credit_card.Update(target);
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
