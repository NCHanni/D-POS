using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncCompany
    {
        private const string LOCAL_TABLE = "company_information";
        private const string ENDPOINT_NAME = "Company";
        private const string ENDPOINT_DESC = "Company Information";

        private readonly Data.DBContext _context;
        public SyncCompany(Data.DBContext context)
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

                var data = new List<Models.Acumatica.CompanyInfo>();
                if (result.Length > 0)
                {
                    data = JsonConvert.DeserializeObject<List<Models.Acumatica.CompanyInfo>>(result);
                }

                if (data.Any())
                {
                    Models.Acumatica.CompanyInfo source = data.First();
                    Models.BusinessPro.company_information target = await _context.company_information.FirstAsync<Models.BusinessPro.company_information>();

                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        target.name = source.Name.ToStringValue();
                        target.name_2 = source.Name2.ToStringValue();
                        target.permit_number = source.PermitNumber.ToStringValue();
                        target.permit_min = source.PermitMin.ToStringValue();
                        target.permit_issued = source.PermitIssued.ToDateTimeValue();
                        target.permit_expiry = source.PermitExpiry.ToDateTimeValue();
                        target.vat_registration_no = source.VatRegistrationNo.ToStringValue();
                        target.business_style = source.BusinessStyle.ToStringValue();
                        target.address = source.Address.ToStringValue(255);
                        target.contact_no = source.ContactNo.ToStringValue();
                        target.line_limit = source.LineLimit.ToIntValue();
                        target.fax_no = source.FaxNo.ToStringValue();
                        target.email_address = source.EmailAddress.ToStringValue();
                        target.website_url = source.WebsiteUrl.ToStringValue();

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
                        data.Count()), 
                    _context);
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync(_context, ENDPOINT_DESC);
            }
        }
    }
}
