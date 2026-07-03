using System.Collections.Generic;

namespace RD_INTEGRATION.Models.Acumatica
{
    public class POS
    {
        public Field BatchRefNbr { get; set; }
        public Field Date { get; set; }
        public Field CashierName { get; set; }
        public Field BranchCode { get; set; }
        public List<Sale> Sale { get; set; }
    }

    public class POSResult
    {
        public Field BatchRefNbr { get; set; }
        public Field Date { get; set; }
    }

    public class Sale
    {
        public Field Code { get; set; }
        public Field CashierSessionID { get; set; }
        public Field TerminalCode { get; set; }
        public Field TransactionDate { get; set; }
        public Field BatchRefNbr { get; set; }

        public Field CustomerCode { get; set; }
        public Field CustomerName { get; set; }
        public Field CustomerTin { get; set; }
        public Field CustomerBusinessStyle { get; set; }
        public Field CustomerAddress { get; set; }
        public Field SalespersonCode { get; set; }
        public Field Salesperson { get; set; }
        public Field CashierCode { get; set; }
        public Field CashierName { get; set; }
        public Field TotalVat { get; set; }
        public Field TotalDiscount { get; set; }
        public Field TotalAmount { get; set; }
        public Field TotalAmountReturned { get; set; }
        public Field VatSales { get; set; }
        public Field VatExemptSales { get; set; }
        public Field ZeroRatedSales { get; set; }
        public Field LessVat { get; set; }
        public Field IsSC { get; set; }
        public Field IsPWD { get; set; }
        public Field ScPwdIdNo { get; set; }
        public Field ScPwdName { get; set; }
        public Field ScPwdDiscount { get; set; }
        public Field ScPwdLessVat { get; set; }
        public Field IsVoid { get; set; }
        public Field VoidCode { get; set; }
        public Field IsAllReturned { get; set; }

        public List<SaleDetail> Detail { get; set; }
        public List<SaleDiscountDetail> DiscountDetail { get; set; }
        public List<SaleCashDetail> Cash { get; set; }
        public List<SaleCardDetail> CreditCardDetail { get; set; }
        public List<SaleMemoDetail> CreditMemoDetail { get; set; }
        public List<SaleGiftDetail> GiftCertDetail { get; set; }
    }

    public class SaleDetail
    {
        public Field LineID { get; set; }
        public Field SaleCode { get; set; }
        public Field ItemCode { get; set; }
        public Field ClassCode { get; set; }
        public Field Price { get; set; }
        public Field Qty { get; set; }
        public Field QtyReturned { get; set; }
        public Field UOM { get; set; }
        public Field QtyPerUom { get; set; }
        public Field RegularDiscount { get; set; }
        public Field Discount { get; set; }
        public Field DiscountAmt { get; set; }
        public Field DiscountPrice { get; set; }
        public Field VAT { get; set; }
        public Field VatAmt { get; set; }
        public Field VatExemptAmt { get; set; }
        public Field Amount { get; set; }
        public Field Vatable { get; set; }
        public Field ZeroRated { get; set; }
        public Field VatExempt { get; set; }
        public Field GiftCert { get; set; }
        public Field SerialLotNo { get; set; }
        public Field AlternateID { get; set; }
    }

    public class SaleDiscountDetail
    {
        public Field SaleCode { get; set; }
        public Field DiscountType { get; set; }
        public Field IdNo { get; set; }
        public Field Name { get; set; }
        public Field Gender { get; set; }
        public Field Birthdate { get; set; }
        public Field IssueDate { get; set; }
        public Field TotalDiscount { get; set; }
        public Field TotalLessVat { get; set; }
    }

    public class SaleCashDetail
    {
        public Field SaleCode { get; set; }
        public Field Amount { get; set; }
        public Field Change { get; set; }
    }

    public class SaleCardDetail
    {
        public Field SaleCode { get; set; }
        public Field TypeCode { get; set; }
        public Field TypeName { get; set; }
        public Field CardNo { get; set; }
        public Field CardHolder { get; set; }
        public Field BankName { get; set; }
        public Field ApprovalCode { get; set; }
        public Field Amount { get; set; }
    }

    public class SaleMemoDetail
    {
        public Field SaleCode { get; set; }
        public Field MemoCode { get; set; }
        public Field Amount { get; set; }
    }

    public class SaleGiftDetail
    {
        public Field SaleCode { get; set; }
        public Field GiftCert { get; set; }
        public Field Description { get; set; }
        public Field Amount { get; set; }
    }
}
