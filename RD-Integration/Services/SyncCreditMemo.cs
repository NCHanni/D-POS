using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncCreditMemo
    {
        private const string LOCAL_TABLE = "credit_memo";
        private const string ENDPOINT_NAME = "Memo";
        private const string ENDPOINT_DESC = "Credit Memo";

        private readonly Data.DBContext _context;
        public SyncCreditMemo(Data.DBContext context)
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
                string parameters = String.Empty;
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

                var list = new List<Models.Acumatica.Memo>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.Memo>>(result);
                }

                if (list.Any())
                {
                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (Models.Acumatica.Memo source in list)
                        {
                            if (source.Type.ToStringValue() != "Credit Memo")
                                continue;

                            var refNbr = source.ReferenceNbr.ToStringValue(30);
                            if (string.IsNullOrEmpty(refNbr))
                                continue;

                            if (await _context.credit_memo.AnyAsync(m => m.document_no == refNbr))
                                continue;

                            var target = new Models.BusinessPro.credit_memo();
                            target.document_no = refNbr;
                            target.document_date = source.Date.ToDateTimeValue();
                            target.customer_no = source.Customer.ToStringValue(30);
                            target.customer_name = string.Empty;
                            target.external_document_no = string.Empty;
                            target.amount = source.TotalAmount.ToDecimalValue();

                            _context.credit_memo.Add(target);
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
