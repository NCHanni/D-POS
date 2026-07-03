using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncCustomer
    {
        private const string LOCAL_TABLE = "customer";
        private const string ENDPOINT_DESC = "Customer";
        private const string ENDPOINT_GET = "CustomerList";
        //private const string ENDPOINT_PUT = "Customer";
        private const string DEFAULT_ATC = "ACU_ATC"; //flag

        private readonly Data.DBContext _context;
        public SyncCustomer(Data.DBContext context)
        {
            _context = context;
        }

        public async Task Run(bool manual = false)
        {
            DateTimeOffset startRunTime = DateTimeOffset.Now;
            Models.RD_Preference preference = await _context.rd_preference.FirstOrDefaultAsync();
            Models.BusinessPro.settings_sync setting = await _context.settings_sync.Where(w => w.table_name == LOCAL_TABLE).FirstAsync();
            Models.BusinessPro.settings_preference prefAtc = await _context.settings_preference.Where(w => w.flag == DEFAULT_ATC).FirstAsync();
            Models.BusinessPro.company_information company = _context.company_information.First<Models.BusinessPro.company_information>();

            try
            {
                DateTime? timestamp = setting.timestamp;

                /*List<Models.BusinessPro.customer> customers;

                if (timestamp != null)
                    customers = await _context.customer.Where(c => c.is_active == "True" && c.timestamp > Convert.ToDateTime(timestamp)).ToListAsync();
                else
                    customers = await _context.customer.Where(c => c.is_active == "True").ToListAsync();

                if (customers.Any())
                {
                    using var trans = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (Models.BusinessPro.customer cust in customers)
                        {
                            var acuCustomer = new Models.Acumatica.Customer
                            {
                                AccountName = new Models.Field(cust.name),
                                DefaultATC = new Models.Field(cust.default_atc),
                                SendDunningLettersbyEmail = new Models.Field(false.ToString()),
                                Birthdate = new Models.Field(cust.birthdate == null ? "" : Convert.ToDateTime(cust.birthdate).ToString()),
                                BusinessStyle = new Models.Field(cust.business_style),
                                DateIssued = new Models.Field(cust.issued_date == null ? "" : Convert.ToDateTime(cust.issued_date).ToString()),
                                DiscountType = new Models.Field(cust.discount_type),
                                Gender = new Models.Field(cust.gender),
                                POSCustomer = new Models.Field(true.ToString()),
                                SCPWDIdNo = new Models.Field(cust.sc_pwd_no),
                                TIN = new Models.Field(cust.tin),
                                Address1 = new Models.Field(string.IsNullOrEmpty(cust.address1) ? cust.address : cust.address1),
                                Address2 = new Models.Field(cust.address2),
                                City = new Models.Field(cust.city)
                            };

                            if (cust.code == cust.codename)
                            {
                                acuCustomer.DefaultATC = new Models.Field(prefAtc.value);

                                using REST rs = new REST(preference);
                                var customerResponse = await rs.PutAsync(ENDPOINT_PUT, JsonConvert.SerializeObject(acuCustomer));

                                var customer = JsonConvert.DeserializeObject<Models.Acumatica.Customer>(customerResponse);
                                cust.codename = customer.CustomerID.ToStringValue();

                                _context.customer.Update(cust);
                            }
                            else
                            {
                                string filter = "$filter=CustomerID eq '" + cust.codename + "'";
                                acuCustomer.CustomerID = new Models.Field(cust.codename);

                                using REST rs = new REST(preference);
                                await rs.PutAsync(ENDPOINT_PUT, JsonConvert.SerializeObject(acuCustomer), filter);
                            }
                        }

                        setting.timestamp = DateTime.Now;
                        await _context.SaveChangesAsync();
                        await trans.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        await ex.LogErrorAsync(_context, string.Format("{0}-PUSH",ENDPOINT_DESC));
                    }
                }*/

                //-------------------------------------------------------------------------------------------

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
                            result = await rs.GetAsync(ENDPOINT_GET, parameters);
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

                var list = new List<Models.Acumatica.CustomerList>();
                if (result.Length > 0)
                {
                    list = JsonConvert.DeserializeObject<List<Models.Acumatica.CustomerList>>(result);
                }

                if (list.Any())
                {
                    using var scope = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (var source in list)
                        {
                            var isNew = false;
                            var address = string.Empty;
                            var city = source.City.ToStringValue();
                            var target = new Models.BusinessPro.customer();
                            var targets = _context.customer.Where(c => c.codename == source.CustomerID.value);

                            if (await targets.AnyAsync())
                                target = await targets.FirstAsync();
                            else
                                isNew = true;

                            address = source.AddressLine2.ToStringValue();
                            address = source.AddressLine1.ToStringValue() + (string.IsNullOrWhiteSpace(address) ? "" : " " + address);
                            city = (string.IsNullOrWhiteSpace(city) ? "" : " " + city);
                                                       
                            target.codename = source.CustomerID.ToStringValue();
                            target.name = source.CustomerName.ToStringValue();
                            target.address = address + city;
                            target.address1 = source.AddressLine1.ToStringValue();
                            target.address2 = source.AddressLine2.ToStringValue();
                            target.city = source.City.ToStringValue(30);
                            target.address1 = source.AddressLine1.ToStringValue();
                            target.contact_no = source.Phone1.ToStringValue();
                            target.tin = source.TIN.ToStringValue();
                            target.business_style = source.BusinessStyle.ToStringValue();
                            target.discount_type = source.DiscountType.ToStringValue(30);
                            target.gender = source.Gender.ToStringValue();
                            target.birthdate = source.Birthdate.ToDateTimeValue();
                            target.sc_pwd_no = source.SCPWDIdNo.ToStringValue();
                            target.issued_date = source.DateIssued.ToDateTimeValue();
                            target.allow_discount = (!string.IsNullOrWhiteSpace(source.DiscountType.ToStringValue())).ToString();
                            target.is_active = true.ToString();
                            target.default_atc = string.Empty; //unused
                            target.timestamp = DateTime.Now;

                            if (isNew)
                            {
                                string codeSQL = 
                                    "SELECT 'C-' + dbo.LeadingZero(6, ISNULL(COALESCE(MAX(CAST(RIGHT(code, 6) AS INTEGER)), 0) + 1, 1)) AS [code] FROM [customer]";

                                Models.BusinessPro.customer_code customerCode = 
                                    await _context.customer_code.FromSqlRaw(codeSQL).FirstOrDefaultAsync();

                                target.code = customerCode.code;
                                _context.customer.Add(target);
                                await _context.SaveChangesAsync();
                            }
                            else
                                _context.customer.Update(target);
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
