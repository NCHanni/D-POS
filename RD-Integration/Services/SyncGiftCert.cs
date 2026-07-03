using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncGiftCert
    {
        private const string LOCAL_TABLE = "gift_certificate";
        private const string ENDPOINT_NAME = "GiftCert";
        private const string ENDPOINT_DESC = "Gift Certificate";
        private const string ENDPOINT_PUT = "GiftCert"; //edit

        private readonly Data.DBContext _context;
        public SyncGiftCert(Data.DBContext context)
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
                List<Models.BusinessPro.gift_certificate> giftCerts;
                DateTime? timestamp = setting.timestamp;

                if (timestamp != null)
                    giftCerts = await _context.gift_certificate.Where(gc => gc.timestamp >= Convert.ToDateTime(timestamp)).ToListAsync();
                else
                    giftCerts = await _context.gift_certificate.ToListAsync();

                if (giftCerts.Any())
                {
                    using var trans = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        using (REST rs = new REST(preference))
                        {
                            foreach (Models.BusinessPro.gift_certificate gc in giftCerts)
                            {

                                string filter = "$filter=Code eq '" + gc.code + "'";

                                string gcResponse = await rs.GetAsync(ENDPOINT_NAME, filter);

                                List<Models.Acumatica.GiftCert> existingGiftCert = JsonConvert.DeserializeObject<List<Models.Acumatica.GiftCert>>(gcResponse);

                                if(existingGiftCert.Any())
                                {
                                    var acuGiftCert = new Models.Acumatica.GiftCert
                                    {
                                        //Code = new Models.Field(gc.code),
                                        //Description = new Models.Field(gc.description),
                                        //ExpiryDate = new Models.Field(Convert.ToDateTime(gc.expiry_date).ToString("MM/dd/yyyy")),
                                        //Amount = new Models.Field(gc.amount.ToString()),
                                        Active = new Models.Field(gc.is_active),
                                        Sold = new Models.Field(gc.is_sold),
                                        Used = new Models.Field(gc.is_used)
                                    };

                                    await rs.PutAsync(ENDPOINT_PUT, JsonConvert.SerializeObject(acuGiftCert), filter);
                                }
                            }
                        }

                        setting.timestamp = DateTime.Now;
                        await _context.SaveChangesAsync();
                        await trans.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        await ex.LogErrorAsync(_context,string.Format("{0}-PUSH", ENDPOINT_DESC));
                    }
                }
                else
                {
                    setting.timestamp = DateTime.Now;
                }

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

                var list = new List<Models.Acumatica.GiftCert>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.GiftCert>>(result);
                }

                if (list.Any())
                {
                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (Models.Acumatica.GiftCert source in list)
                        {
                            var isNew = false;
                            var target = new Models.BusinessPro.gift_certificate();
                            var targets = _context.gift_certificate.Where(c => c.code == source.Code.value);

                            if (await targets.AnyAsync())
                                target = await targets.FirstAsync();
                            else
                            {
                                isNew = true;
                                target.id = 0;
                            }

                            target.code = source.Code.ToStringValue();
                            target.description = source.Description.ToStringValue();
                            target.expiry_date = source.ExpiryDate.ToDateTimeValue();
                            target.amount = source.Amount.ToDecimalValue();
                            target.is_active = source.Active.ToBooleanString();
                            target.is_sold = source.Sold.ToBooleanString();
                            target.is_used = source.Used.ToBooleanString();
                            target.timestamp = DateTime.Now;

                            if (isNew)
                                _context.gift_certificate.Add(target);
                            else
                                _context.gift_certificate.Update(target);
                        }

                        setting.timestamp = DateTime.Now;
                        await _context.SaveChangesAsync();
                        await scope.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await scope.RollbackAsync();
                        await ex.LogErrorAsync(_context, string.Format("{0}-PULL", ENDPOINT_DESC));
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
