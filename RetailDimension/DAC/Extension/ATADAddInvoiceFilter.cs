using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;
using PX.Objects.SO.DAC.Unbound;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDAddInvoiceFilter : PXCacheExtension<AddInvoiceFilter>
    {
        public static bool IsActive() { return true; }

        #region UsrATRDSaleCode
        [PXString(12, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.SaleCode)]
        [PXSelector(typeof(Search2<ATRDBatchSO.saleCode, InnerJoin<ATRDSale, On<ATRDSale.code, Equal<ATRDBatchSO.saleCode>>>>), typeof(ATRDBatchSO.saleCode), typeof(ATRDSale.transactionDate), typeof(ATRDSale.customerName))]
        public string UsrATRDSaleCode { get; set; }
        public abstract class usrATRDSaleCode : BqlString.Field<usrATRDSaleCode> { }
        #endregion

        //#region UsrATRDOrderNbr
        //[PXString(15, IsUnicode = true)]
        //[PXUIField(DisplayName = ATRDMessages.OrderNbr)]
        //[PXFormula(typeof(Selector<usrATRDSaleCode, ATRDBatchSO.orderNbr>))]
        //[PXFormula(typeof(Default<usrATRDSaleCode>))]
        //public string UsrATRDOrderNbr { get; set; }
        //public abstract class usrATRDOrderNbr : BqlString.Field<usrATRDOrderNbr> { }
        //#endregion
    }
}
