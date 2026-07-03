using PX.Data;
using PX.Data.BQL;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.Objects.GL;
using RetailDimension.DAC.Base;
using RetailDimension.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RetailDimension.Attributes.Base.AggregateAttribute;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RD-Sale")]
    [PXPrimaryGraph(typeof(ATRDSaleEntry))]
    public class ATRDSale : Table, IBqlTable
    {
        #region BatchRefNbr 
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.BatchRefNbr)]
        [PXDBDefault(typeof(ATRDBatchSale.refNbr))]
        [PXParent(typeof(Select<ATRDBatchSale, Where<ATRDBatchSale.refNbr, Equal<Current<ATRDSale.batchRefNbr>>>>))]
        public virtual string BatchRefNbr { get; set; }
        public abstract class batchRefNbr : BqlString.Field<batchRefNbr> { }
        #endregion

        #region BranchID
        [Branch(DisplayName = ATRDMessages.Branch)]
        [PXUIField(Enabled = false)]
       // [PXDBDefault(typeof(ATRDBatchSale.branchID))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region Code 
        //code varchar(12)
        [PXDBString(12, IsKey = true, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Code)]
        [PXRestrictor(typeof(Where<branchID.IsEqual<AccessInfo.branchID.FromCurrent>>),
            ATRDMessages.RestrictBatchByBranch)]
        [PXDefault]
        public virtual string Code { get; set; }
        public abstract class code : BqlString.Field<code> { }
        #endregion

        #region CashierSessionID
        //cashier_session_id bigint
        [PXDBLong]
        [PXUIField(DisplayName = ATRDMessages.CashierSessionID)]
        public virtual long? CashierSessionID { get; set; }
        public abstract class cashierSessionID : BqlLong.Field<cashierSessionID> { }
        #endregion

        #region TerminalCode
        //terminal_code varchar(255) 
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.TerminalCode)]
        public virtual string TerminalCode { get; set; }
        public abstract class terminalCode : BqlString.Field<terminalCode> { }
        #endregion

        #region TransactionDate
        //transaction_date datetime2(7)
        [PXDBDate()]
        [PXUIField(DisplayName = ATRDMessages.TransactionDate)]
        public virtual DateTime? TransactionDate { get; set; }
        public abstract class transactionDate : BqlDateTime.Field<transactionDate> { }
        #endregion

        #region CustomerCode
        //customer_code varchar(8) 
        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerCode)]
        public virtual string CustomerCode { get; set; }
        public abstract class customerCode : BqlString.Field<customerCode> { }
        #endregion

        #region CustomerName
        //customer_name varchar(255) 
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerName)]
        public virtual string CustomerName { get; set; }
        public abstract class customerName : BqlString.Field<customerName> { }
        #endregion

        #region CustomerTin
        //customer_tin varchar(255) 
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerTin)]
        public virtual string CustomerTin { get; set; }
        public abstract class customerTin : BqlString.Field<customerTin> { }
        #endregion

        #region CustomerBusinessStyle
        //customer_business_style varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerBusinessStyle)]
        public virtual string CustomerBusinessStyle { get; set; }
        public abstract class customerBusinessStyle : BqlString.Field<customerBusinessStyle> { }
        #endregion

        #region CustomerAddress
        //customer_address varchar(500) 
        [PXDBString(500, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerAddress)]
        public virtual string CustomerAddress { get; set; }
        public abstract class customerAddress : BqlString.Field<customerAddress> { }
        #endregion

        #region SalespersonCode
        [PXDBInt]
        [PXUIField(DisplayName = ATRDMessages.SalespersonCode)] 
        [PXSelector(typeof(Search<SalesPerson.salesPersonID>),
            SubstituteKey = typeof(SalesPerson.salesPersonCD),
            DescriptionField = typeof(SalesPerson.descr))]
        public virtual int? SalespersonCode { get; set; }
        public abstract class salespersonCode : BqlInt.Field<salespersonCode> { }
        #endregion

        #region Salesperson
        //salesperson varchar(255) 
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Salesperson)]
        public virtual string Salesperson { get; set; }
        public abstract class salesperson : BqlString.Field<salesperson> { }
        #endregion

        #region CashierCode
        //cashier_code varchar(5) 
        [PXDBString(5, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CashierCode)]
        public virtual string CashierCode { get; set; }
        public abstract class cashierCode : BqlString.Field<cashierCode> { }
        #endregion

        #region CashierName
        //cashier_name varchar(255) 
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CashierName)]
        public virtual string CashierName { get; set; }
        public abstract class cashierName : BqlString.Field<cashierName> { }
        #endregion

        #region TotalVat
        //total_vat numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.TotalVat)]
        public virtual decimal? TotalVat { get; set; }
        public abstract class totalVat : BqlDecimal.Field<totalVat> { }
        #endregion

        #region TotalDiscount
        //total_discount numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.TotalDiscount)]
        public virtual decimal? TotalDiscount { get; set; }
        public abstract class totalDiscount : BqlDecimal.Field<totalDiscount> { }
        #endregion

        #region TotalAmount
        //total_amount numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.TotalAmount)]
        public virtual decimal? TotalAmount { get; set; }
        public abstract class totalAmount : BqlDecimal.Field<totalAmount> { }
        #endregion

        #region TotalAmountReturned
        //total_amount_returned numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.TotalAmountReturned)]
        public virtual decimal? TotalAmountReturned { get; set; }
        public abstract class totalAmountReturned : BqlDecimal.Field<totalAmountReturned> { }
        #endregion

        #region VatSales
        //vat_sales numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.VatSales)]
        public virtual decimal? VatSales { get; set; }
        public abstract class vatSales : BqlDecimal.Field<vatSales> { }
        #endregion

        #region VatExemptSales
        //vat_exempt_sales numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.VatExemptSales)]
        public virtual decimal? VatExemptSales { get; set; }
        public abstract class vatExemptSales : BqlDecimal.Field<vatExemptSales> { }
        #endregion

        #region ZeroRatedSales
        //zero_rated_sales numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.ZeroRatedSales)]
        public virtual decimal? ZeroRatedSales { get; set; }
        public abstract class zeroRatedSales : BqlDecimal.Field<zeroRatedSales> { }
        #endregion

        #region LessVat
        //less_vat numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.LessVat)]
        public virtual decimal? LessVat { get; set; }
        public abstract class lessVat : BqlDecimal.Field<lessVat> { }
        #endregion

        #region IsSC
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsSC)]
        //is_sc varchar(5)
        public virtual Boolean? IsSC { get; set; }
        public abstract class isSC : BqlBool.Field<isSC> { }
        #endregion

        #region IsPWD
        //is_pwd varchar(5)
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsPWD)]
        public virtual Boolean? IsPWD { get; set; }
        public abstract class isPWD : BqlBool.Field<isPWD> { }
        #endregion

        #region ScPwdIdNo
        //sc_pwd_id_no varchar(255) 
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ScPwdIdNo)]
        public virtual string ScPwdIdNo { get; set; }
        public abstract class scPwdIdNo : BqlString.Field<scPwdIdNo> { }
        #endregion

        #region ScPwdName
        //sc_pwd_name varchar(255) 
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ScPwdName)]
        public virtual string ScPwdName { get; set; }
        public abstract class scPwdName : BqlString.Field<scPwdName> { }
        #endregion

        #region ScPwdDiscount
        //sc_pwd_discount numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.ScPwdDiscount)]
        public virtual decimal? ScPwdDiscount { get; set; }
        public abstract class scPwdDiscount : BqlDecimal.Field<scPwdDiscount> { }
        #endregion

        #region ScPwdLessVat
        //sc_pwd_less_vat numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.ScPwdLessVat)]
        public virtual decimal? ScPwdLessVat { get; set; }
        public abstract class scPwdLessVat : BqlDecimal.Field<scPwdLessVat> { }
        #endregion

        #region IsVoid
        //is_void varchar(5)
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsVoid)]
        public virtual Boolean? IsVoid { get; set; }
        public abstract class isVoid : BqlBool.Field<isVoid> { }
        #endregion

        #region VoidCode
        //void_code varchar(255) 
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.VoidCode)]
        public virtual string VoidCode { get; set; }
        public abstract class voidCode : BqlString.Field<voidCode> { }
        #endregion

        #region IsAllReturned
        //is_all_returned varchar(5)
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsAllReturned)]
        public virtual Boolean? IsAllReturned { get; set; }
        public abstract class isAllReturned : BqlBool.Field<isAllReturned> { }
        #endregion

        #region OrderNbr
        [PXString(15)]
        [PXUIField(DisplayName = ATRDMessages.OrderNbr, Enabled = false)]
        [PXFormula(typeof(Parent<ATRDBatchSale.orderNbr>))]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : BqlString.Field<orderNbr> { }
        #endregion
    }
}
