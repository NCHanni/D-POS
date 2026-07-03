using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RD_INTEGRATION.Helpers;
using RD_INTEGRATION.Models.Acumatica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Services
{
    public class SyncSale
    {
        private const string LOCAL_TABLE = "sale";
        private const string ENDPOINT_NAME = "POS";
        private const string ENDPOINT_ERROR = "Sale Cashier";
        private const string ENDPOINT_DESC = "Sale Cashier - {0}";
        private const string FALSE = "False";

        private readonly Data.DBContext _context;
        public SyncSale(Data.DBContext context)
        {
            _context = context;
        }

        public async Task Run(bool manual = false)
        {
            DateTimeOffset startRunTime = DateTimeOffset.Now;
            Models.RD_Preference preference = await _context.rd_preference.FirstOrDefaultAsync();
            Models.BusinessPro.settings_sync setting = await _context.settings_sync.Where(w => w.table_name == LOCAL_TABLE).FirstAsync();
            Models.BusinessPro.company_information company = await _context.company_information.FirstAsync<Models.BusinessPro.company_information>();

            try
            {
                DateTime[] dates = await _context.sale.Where(w => w.is_synced == FALSE && w.is_void == FALSE).GroupBy(g => g.transaction_date.Date).Select(s => s.Key).ToArrayAsync();
                string[] cashiers = await _context.sale.Where(w => w.is_synced == FALSE && w.is_void == FALSE).GroupBy(g => g.cashier_code).Select(s => s.Key).ToArrayAsync();

                foreach (DateTime date in dates)
                {
                    foreach (string cashier in cashiers)
                    {
                        bool batchDetailsSet = false;

                        Models.Acumatica.POS pos = new Models.Acumatica.POS
                        {
                            BatchRefNbr = new Models.Field() { value = "<NEW>" },
                            CashierName = new Models.Field() { value = cashier }, //cashier code by default
                            BranchCode = new Models.Field() { value = preference.Branch },
                            Sale = new List<Sale>()
                        };

                        IEnumerable<Models.BusinessPro.sale> sales = _context.sale.Where(w => w.is_synced == FALSE && w.is_void == FALSE && w.transaction_date.Date == date && w.cashier_code == cashier);
                        using var scope = await _context.Database.BeginTransactionAsync();

                        foreach (IEnumerable<Models.BusinessPro.sale> batch in sales.Batch(company.line_limit))
                        {
                            var recordsAffected = 0;

                            foreach (Models.BusinessPro.sale sale in batch)
                            {
                                if (!batchDetailsSet)
                                {
                                    pos.Date = new Models.Field() { value = sale.transaction_date.Date.AddHours(preference.TransactionDateOffset).ToDateString() };
                                    pos.CashierName = new Models.Field() { value = sale.cashier_name }; //update to name
                                    batchDetailsSet = true;
                                }

                                Models.Field salespersonCode = null;
                                if (!string.IsNullOrWhiteSpace(sale.salesperson_code))
                                {
                                    salespersonCode = new Models.Field() { value = sale.salesperson_code };
                                }

                                if (sale.total_amount == 0)
                                {
                                    continue;
                                }

                                Models.Acumatica.Sale transaction = new Models.Acumatica.Sale
                                {
                                    //Sale
                                    Code = new Models.Field() { value = sale.code },
                                    CashierSessionID = new Models.Field() { value = sale.cashier_session_id.ToString() },
                                    TerminalCode = new Models.Field() { value = sale.terminal_code },
                                    TransactionDate = new Models.Field() { value = sale.transaction_date.Date.AddHours(preference.TransactionDateOffset).ToDateString() },
                                    BatchRefNbr = pos.BatchRefNbr,
                                    CustomerCode = new Models.Field() { value = sale.customer_code },
                                    CustomerName = new Models.Field() { value = sale.customer_name },
                                    CustomerTin = new Models.Field() { value = sale.customer_tin },
                                    CustomerBusinessStyle = new Models.Field() { value = sale.customer_business_style },
                                    CustomerAddress = new Models.Field() { value = sale.customer_address },
                                    SalespersonCode = salespersonCode,
                                    Salesperson = new Models.Field() { value = sale.salesperson },
                                    CashierCode = new Models.Field() { value = sale.cashier_code },
                                    CashierName = new Models.Field() { value = sale.cashier_name },
                                    TotalVat = new Models.Field() { value = sale.total_vat.ToString() },
                                    TotalDiscount = new Models.Field() { value = sale.total_discount.ToString() },
                                    TotalAmount = new Models.Field() { value = sale.total_amount.ToString() },
                                    TotalAmountReturned = new Models.Field() { value = sale.total_amount_returned.ToString() },
                                    VatSales = new Models.Field() { value = sale.vat_sales.ToString() },
                                    VatExemptSales = new Models.Field() { value = sale.vat_exempt_sales.ToString() },
                                    ZeroRatedSales = new Models.Field() { value = sale.zero_rated_sales.ToString() },
                                    LessVat = new Models.Field() { value = sale.less_vat.ToString() },
                                    IsSC = new Models.Field() { value = sale.is_sc.ToBoolean().ToString() },
                                    IsPWD = new Models.Field() { value = sale.is_pwd.ToBoolean().ToString() },
                                    ScPwdIdNo = new Models.Field() { value = sale.sc_pwd_id_no },
                                    ScPwdName = new Models.Field() { value = sale.sc_pwd_name },
                                    ScPwdDiscount = new Models.Field() { value = sale.sc_pwd_discount.ToString() },
                                    ScPwdLessVat = new Models.Field() { value = sale.sc_pwd_less_vat.ToString() },
                                    IsVoid = new Models.Field() { value = sale.is_void.ToBoolean().ToString() },
                                    VoidCode = new Models.Field() { value = sale.void_code },
                                    IsAllReturned = new Models.Field() { value = sale.is_all_returned.ToBoolean().ToString() },

                                    //Details
                                    Detail = new List<Models.Acumatica.SaleDetail>()
                                };

                                List<Models.BusinessPro.sale_details> details = await _context.sale_details.Where(w => w.sale_code == sale.code).ToListAsync();

                                foreach (Models.BusinessPro.sale_details detail in details)
                                {
                                    Models.Acumatica.SaleDetail saleDetail = new Models.Acumatica.SaleDetail
                                    {
                                        LineID = new Models.Field() { value = detail.item_line_id.ToString() },
                                        SaleCode = new Models.Field() { value = detail.sale_code },
                                        ItemCode = new Models.Field() { value = detail.item_code },
                                        ClassCode = new Models.Field() { value = detail.class_code },
                                        Price = new Models.Field() { value = detail.price.ToString() },
                                        Qty = new Models.Field() { value = detail.qty.ToString() },
                                        QtyReturned = new Models.Field() { value = detail.qty_returned.ToString() },
                                        UOM = new Models.Field() { value = detail.unit_of_measure },
                                        QtyPerUom = new Models.Field() { value = detail.qty_per_uom.ToString() },
                                        RegularDiscount = new Models.Field() { value = detail.is_regular_discount.ToBoolean().ToString() },
                                        Discount = new Models.Field() { value = detail.discount_percent.ToString() },
                                        DiscountAmt = new Models.Field() { value = detail.discount_amount.ToString() },
                                        DiscountPrice = new Models.Field() { value = detail.discounted_price.ToString() },
                                        VAT = new Models.Field() { value = detail.vat_percent.ToString() },
                                        VatAmt = new Models.Field() { value = detail.vat_amount.ToString() },
                                        VatExemptAmt = new Models.Field() { value = detail.vat_exempt_amount.ToString() },
                                        Amount = new Models.Field() { value = detail.amount.ToString() },
                                        Vatable = new Models.Field() { value = detail.is_vatable.ToBoolean().ToString() },
                                        ZeroRated = new Models.Field() { value = detail.is_zero_rated.ToBoolean().ToString() },
                                        VatExempt = new Models.Field() { value = detail.is_vat_exempt.ToBoolean().ToString() },
                                        GiftCert = new Models.Field() { value = detail.is_gift_certificate.ToString().ToString() },
                                        SerialLotNo = new Models.Field() { value = detail.serial_nbr.ToString().ToString() },
                                        AlternateID = new Models.Field() { value = detail.alternate_id.ToString().ToString() }
                                    };

                                    transaction.Detail.Add(saleDetail);
                                }

                                //Discount
                                List<Models.BusinessPro.sale_discount_details> discountDetails = await _context.sale_discount_details.Where(w => w.sale_code == sale.code).ToListAsync();
                                if(discountDetails.Any())
                                {
                                    transaction.DiscountDetail = new List<Models.Acumatica.SaleDiscountDetail>();
                                    foreach (Models.BusinessPro.sale_discount_details detail in discountDetails)
                                    {
                                        Models.Acumatica.SaleDiscountDetail saleDiscountDetail = new Models.Acumatica.SaleDiscountDetail
                                        {
                                            SaleCode = new Models.Field() { value = detail.sale_code },
                                            DiscountType = new Models.Field() { value = detail.discount_type },
                                            IdNo = new Models.Field() { value = detail.id_no },
                                            Name = new Models.Field() { value = detail.name },
                                            Gender = new Models.Field() { value = detail.gender },
                                            Birthdate = new Models.Field() { value = detail.birthdate.ToDateString(true) },
                                            IssueDate = new Models.Field() { value = detail.issued_date.ToDateString(true) },
                                            TotalDiscount = new Models.Field() { value = detail.total_discount.ToString() },
                                            TotalLessVat = new Models.Field() { value = detail.total_less_vat.ToString() }
                                        };

                                        transaction.DiscountDetail.Add(saleDiscountDetail);
                                    }
                                }

                                //Cash 
                                List<Models.BusinessPro.payment_cash> cashDetails = await _context.payment_cash.Where(w => w.sale_code == sale.code).ToListAsync();
                                if(cashDetails.Any())
                                {
                                    transaction.Cash = new List<Models.Acumatica.SaleCashDetail>();
                                    foreach (Models.BusinessPro.payment_cash detail in cashDetails)
                                    {
                                        Models.Acumatica.SaleCashDetail cashDetail = new Models.Acumatica.SaleCashDetail
                                        {
                                            SaleCode = new Models.Field() { value = detail.sale_code },
                                            Amount = new Models.Field() { value = detail.amount.ToString() },
                                            Change = new Models.Field() { value = detail.change.ToString() }
                                        };

                                        transaction.Cash.Add(cashDetail);
                                    }
                                }

                                //Credit Card
                                List<Models.BusinessPro.payment_credit_card> cardDetails = await _context.payment_credit_card.Where(w => w.sale_code == sale.code).ToListAsync();
                                if(cardDetails.Any())
                                {
                                    transaction.CreditCardDetail = new List<Models.Acumatica.SaleCardDetail>();
                                    foreach (Models.BusinessPro.payment_credit_card detail in cardDetails)
                                    {
                                        Models.Acumatica.SaleCardDetail cardDetail = new Models.Acumatica.SaleCardDetail
                                        {
                                            SaleCode = new Models.Field() { value = detail.sale_code },
                                            TypeCode = new Models.Field() { value = detail.type_code },
                                            TypeName = new Models.Field() { value = detail.type_name },
                                            CardNo = new Models.Field() { value = detail.card_no },
                                            CardHolder = new Models.Field() { value = detail.card_holder },
                                            BankName = new Models.Field() { value = detail.bank_name },
                                            ApprovalCode = new Models.Field() { value = detail.approval_code },
                                            Amount = new Models.Field() { value = detail.amount.ToString() }
                                        };

                                        transaction.CreditCardDetail.Add(cardDetail);
                                    }
                                }

                                //Memo
                                List<Models.BusinessPro.payment_credit_memo> memoDetails = await _context.payment_credit_memo.Where(w => w.sale_code == sale.code).ToListAsync();
                                if(memoDetails.Any())
                                {
                                    transaction.CreditMemoDetail = new List<Models.Acumatica.SaleMemoDetail>();
                                    foreach (Models.BusinessPro.payment_credit_memo detail in memoDetails)
                                    {
                                        Models.Acumatica.SaleMemoDetail memoDetail = new Models.Acumatica.SaleMemoDetail
                                        {
                                            SaleCode = new Models.Field() { value = detail.sale_code },
                                            MemoCode = new Models.Field() { value = detail.memo_code },
                                            Amount = new Models.Field() { value = detail.amount.ToString() }
                                        };

                                        transaction.CreditMemoDetail.Add(memoDetail);
                                    }
                                }

                                //Gift Cert.
                                List<Models.BusinessPro.payment_gift_certificate> giftDetails = await _context.payment_gift_certificate.Where(w => w.sale_code == sale.code).ToListAsync();
                                if (giftDetails.Any())
                                {
                                    transaction.GiftCertDetail = new List<Models.Acumatica.SaleGiftDetail>();
                                    foreach (Models.BusinessPro.payment_gift_certificate detail in giftDetails)
                                    {
                                        Models.Acumatica.SaleGiftDetail giftDetail = new Models.Acumatica.SaleGiftDetail
                                        {
                                            SaleCode = new Models.Field() { value = detail.sale_code },
                                            GiftCert = new Models.Field() { value = detail.gc_code },
                                            Description = new Models.Field() { value = detail.description },
                                            Amount = new Models.Field() { value = detail.amount.ToString() }
                                        };

                                        transaction.GiftCertDetail.Add(giftDetail);
                                    }
                                }

                                pos.Sale.Add(transaction);
                            } //foreach sale

                            try
                            {
                                string jsonData = JsonConvert.SerializeObject(pos);
                                string result = string.Empty;
                                var posResult = new POSResult();

                                using (REST rs = new REST(preference))
                                {
                                    result = await rs.PutAsync(ENDPOINT_NAME, jsonData);
                                }

                                if (result.Length > 0)
                                {
                                    posResult = JsonConvert.DeserializeObject<POSResult>(result);

                                    if (posResult != null && !string.IsNullOrWhiteSpace(posResult.BatchRefNbr.value))
                                    {
                                        //Update Sync Field
                                        foreach (Models.BusinessPro.sale sale in sales)
                                        {
                                            sale.is_synced = "True";
                                            sale.acu_batch_ref_number = posResult.BatchRefNbr.value;
                                            recordsAffected++;
                                        }

                                        await _context.SaveChangesAsync();
                                    }

                                    await scope.CommitAsync();
                                }
                            }
                            catch
                            {
                                await scope.RollbackAsync();
                                recordsAffected = 0;
                                throw;
                            }

                            await Methods.LogAsync(
                                String.Format(
                                    Messages.RunFormat, 
                                    manual ? Messages.Manual : Messages.Scheduled, 
                                    String.Format(ENDPOINT_DESC, cashier), 
                                    (int)(DateTimeOffset.Now - startRunTime).TotalSeconds, 
                                    recordsAffected), 
                                _context);
                        }
                    }
                }

                setting.timestamp = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync(_context, ENDPOINT_ERROR);
            }           
        }
    }
}
